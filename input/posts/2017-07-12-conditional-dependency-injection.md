Title: Refactoring Conditional Dependency Injection
Published: 2017-07-12 21:32:00 -0500
Image: images/2017-07-12-masthead.png
HeaderInverted: true
Lead: How do you inject multiple implementations of a single interface and distinguish between them?
Tags:
- C#
- .NET
- Software Design
- Refactoring
---

Recently a colleague sent me a question about some advice using [Ninject](http://www.ninject.org/), a popular .NET dependency injection framework. It went something like this:

> I have a console app that does some tasks. Two of the tasks are very similar, they each process a downloaded file. But the process is different depending on the requested download types passed in.
> 
> The way I'm modeling it right now is like this:
> 
> 1. `DownloadProcessor : IDownloadProcessor`
> 2. `SalesDownloader : IDownloader`
> 3. `TrafficDownloader : IDownloader`
> 
> What I'd like to do is have the correct `IDownloader` used by the `DownloadProcessor` depending on incoming arguments.

<!-- More -->

Here's what they showed me:

```cs
public class ProgramImplementation
{
  private readonly IDownloadProcessor _processor;
  private readonly IDownloader _salesDownloader;
  private readonly IDownloader _trafficDownloader;

  public ProgramImplementation(
    [Named("salesDownloader")]   IDownloader salesDownloader, 
    [Named("trafficDownloader")] IDownloader trafficDownloader, 
    IDownloadProcessor processor)
  {
    _processor = processor;
    _salesDownloader = salesDownloader;
    _trafficDownloader = trafficDownloader;
  }
  
  public void Run(string[] args)
  {
    if (args.Contain("sales"))
    {
        _processor.Process(_salesDownloader);
    }
    
    if (args.Contain("traffic"))
    {
        _processor.Process(_trafficDownloader);
    }
  }
}
```

The `IDownloadProcessor` implementation looks like this:

```cs
public class DownloadProcessor : IDownloadProcessor {
    public void Process(IDownloader downloader) {

      // business logic

      downloader.Process();
    }
}
```

The `ProgramImplementation` is constructed via Ninject so the constructor parameters are populated automatically.

## Gimme Some Context

First, what's going on with this?

```cs
[Named("salesDownloader")] IDownloader salesDownloader
```

This is what Ninject refers to as [Contextual Binding](https://github.com/ninject/ninject/wiki/Contextual-Binding).

You can bind the same interface to different concrete classes and attach "metadata" that can be used at injection time:

```cs
_kernel.Bind<IDownloader>().To<SalesDownloader>().Named("salesDownloader");
_kernel.Bind<IDownloader>().To<TrafficDownloader>().Named("trafficDownloader");
```

In the above docs, they say you should avoid this pattern because now the caller has to reference Ninject directly to get what it wants.

Does this work? Sure but my coworker reached out to me "because it felt weird" so let's break it down.

## A Hammer Looking for a Nail

If you wanted to still use Ninject to achieve this, there's an alternative way to bind the same interface to specific concrete classes *without* having the calling class take a direct dependency on Ninject. 

You have to create two implementations of `IDownloadProcessor`:

```cs
public interface IDownloadProcessor {
  void Process();
}

public abstract class BaseDownloadProcessor : IDownloadProcessor {
  
  private IFileDownloader _downloader;
  public BaseDownloadProcessor(IFileDownloader downloader) {
    _downloader = downloader;
  }

  public void Process() {
    // common logic
    // ...

    // specific downloader logic
    _downloader.Process();
  }
}

// empty implementation classes

public class SalesDownloadProcessor : BaseDownloadProcessor {

  public SalesDownloadProcessor(IFileDownloader downloader)
    : base(downloader) {

  }
}

public class TrafficDownloadProcessor : IDownloadProcessor {
  public TrafficDownloadProcessor(IFileDownloader downloader)
    : base(downloader) {
    
  }
}
```

This adds an two extra classes (one holding common base implementation logic) to what we had before but at least we can inject the right downloader that we need *and* we've simplified the interface. 

We can now bind the specific implementation to the specific processor:

```cs
_kernel.Bind<IDownloader>().To<SalesDownloader>()
  .WhenInjectedInto(typeof(SalesDownloadProcessor));

_kernel.Bind<IDownloader>().To<TrafficDownloader>()
  .WhenInjectedInto(typeof(TrafficDownloadProcessor));
```

Great! We removed a direct reference to Ninject in our `ProgramImplementation`:

```cs
public class ProgramImplementation
{
  private readonly SalesDownloadProcessor _salesProcessor;
  private readonly TrafficDownloadProcessor _trafficProcessor;

  public ProgramImplementation(
    SalesDownloadProcessor salesProcessor,
    TrafficDownloadProcessor trafficProcessor)
  {
    _salesProcessor = salesProcessor;
    _trafficProcessor = trafficProcessor;
  }
  
  public void Run(string[] args)
  {
    if (args.Contain("sales"))
    {
        _salesProcessor.Process();
    }
    
    if (args.Contain("traffic"))
    {
        _trafficProcessor.Process();
    }
  }
}
```

This is still a little smelly, wouldn't you agree? 

<iframe src="https://giphy.com/embed/PsvD1p3IthN96" width="480" height="270" frameBorder="0" class="giphy-embed" allowFullScreen></iframe>

I thought so. Here's what smells:

- The `ProgramImplementation` has to be aware of **both** concrete implementations of `IDownloadProcessor` in TWO places, one in the constructor and one when deciding what to call
- We still have duplicate code in two similar conditional branches and if we add more, it'll keep growing
- Lastly and most importantly, **we are deciding** what downloader is paired with what processor and I think this is the most egregious problem here

Overall this will work but both download processors are identical and we've introduced an abstract class. There's no value in having two empty classes lying around--its solely due to Ninject. In other words, we're just a hammer looking for a nail... maybe we can solve this by rethinking the problem.

## Colocate Behavior and Data

The solution I proposed to my coworker was to *fuhgeddabout* Ninject because I don't think this should be Ninject's concern or involve black box binding incantation at all.

Ask yourself this question: who should decide what an `IDownloader` can handle? Right now, it's the `ProgramImplementation` but I'd argue that it's the *downloader* who knows if it can process a download type--is it *really* the top-level program's concern who handles what? I don't think so.

Encapsulation is about keeping behavior and data close together. In this instance, data are the arguments being passed to the application and the behavior is the processing of the download inside the `IDownloader`.

With that in mind, let's move the decision making farther down, closer to where it needs to be used.

First, let's start at the top and pass the args into the processor:

```cs
public class ProgramImplementation
{
  private readonly IDownloadProcessor _processor;

  public ProgramImplementation(
    IDownloadProcessor processor)
  {
    _processor = processor;
  }
  
  public void Run(string[] args)
  {
    if (args.Contain("sales"))
    {
        _processor.Process("sales");
    }
    
    if (args.Contain("traffic"))
    {
        _processor.Process("traffic");
    }
  }
}
```

Right away we can see an issue. The usage of `_processor.Process` multiple times indicates we should allow passing in multiple values.

Let's modify the `ProgramImplementation` and the `DownloadProcessor` to work with an array of download types:

```cs
public class ProgramImplementation
{
  private readonly IDownloadProcessor _processor;

  public ProgramImplementation(
    IDownloadProcessor processor)
  {
    _processor = processor;
  }
  
  public void Run(string[] args)
  {
    _processor.Process(args);
  }
}

public class DownloadProcessor : IDownloadProcessor {

  private readonly IEnumerable<IDownloader> _downloaders;

  public DownloadProcessor(IEnumerable<IDownloader> downloaders) {
    _downloaders = downloaders;
  }

  public void Process(string[] types) {
    // ???
  }
}
```

It's certainly getting simpler. We're still using Ninject to inject the available downloaders into the processor but we aren't relying on contextual binding.

At this point, you might ask, "Does the processor decide which downloader handles the types? Will I need a switch statement here or something?" Nope, remember we want to push the behavior and data closer together. 

Let's make the change that will colocate the logic inside the `IDownloader` implementations:

```cs
public class DownloadProcessor : IDownloadProcessor {

  private readonly IEnumerable<IDownloader> _downloaders;

  public DownloadProcessor(IEnumerable<IDownloader> downloaders) {
    _downloaders = downloaders;
  }

  public void Process(string[] types) {
    foreach(var downloader in _downloaders) {
      if (downloader.CanProcess(types)) {
        downloader.Process();
      }
    }
  }
}

public interface IDownloader {
  bool CanProcess(string[] types);
  void Process();
}

public class SalesDownloader : IDownloader {
  public bool CanProcess(string[] types) {
    return types.IndexOf("sales") > -1;
  }

  public void Process() {
    // etc.
  }
}

public class TrafficDownloader : IDownloader {
  public bool CanProcess(string[] types) {
    return types.IndexOf("traffic") > -1;
  }

  public void Process() {
    // etc.
  }
}
```

That's more like it. 

We're iterating through each downloader in the processor and processing the download if and only if the handler can handle it (i.e. the decision is left to the downloader).

`IDownloader.CanProcess` provides a contract that says we have to specify what a downloader can handle. Could we just as easily have done `Process(string[] types)`? Yes, we could but this way we are *guaranteeing* that a downloader *must* tell us whether it could handle any of the types we provide. Without this, there'd be no way to enforce this contract. Furthermore, having a check method allows us to ask, "How would I know if a downloader *didn't* handle my download type?".

This is looking good. At this point, you could call it a refactoring job well done. Now it's time for the bonus round to snag those extra maintainability points.

## ToString or Not ToString?

My philosophy is to consolidate multiple "sources of truth" to a single source. Right now for me to figure out what download types are acceptable to pass to this application I have to look at each `IDownloader` implementation, inside the `CanProcess` functions. Yuck!

We could use constants and have them on the `ProgramImplementation` to make acceptable download types more discoverable. But if we switch to an enumeration instead of passing raw strings we'll have a strongly typed and enforceable contract. **BONUS:** Enums support flags so we can represent multiple values without an array!

```cs
// Source of truth for all download types
enum DownloadType {
  Sales,
  Traffic
}

class ProgramImplementation {
  private readonly IDownloadProcessor _processor;

  public ProgramImplementation(
    IDownloadProcessor processor) {
    _processor = processor;
  }

  public void Run(string[] args) {

    foreach (var arg in args) {

      // Parse the argument directly as an enum,
      // ensuring we always pass valid values
      DownloadType type;
      if (!Enum.TryParse(arg, out type)) {
        throw new ArgumentException($"Type '{arg}' is not a valid download type");
      }

      _processor.Process(type);
    }
  }
}
```

Instead of comparing strings, we're now enforcing consistency--we work with the enums and translate strings to `DownloadType` so that we can validate values.

Pretty good! But there's a problem. We went back to passing a single download type. Not cool! It would be great if we could translate that string list into a flagged enum value...

## Beam Me Opts, Scotty!

Not sure if that quote works but I digress. Let's leverage a nice open source package to do some options parsing for us so our CLI application can be more flexible and robust.

[Commandline](https://github.com/gsscoder/commandline) is a fantastic Nuget package for the job. 

We can use Commandline to take in a list of the enum values and convert them into a flag. Here's how we use it to simplify our logic further:

```cs
[Flags]
enum DownloadType {
  Sales,
  Traffic
}

class Options {
  [Option('t', "types", Required = true,
    HelpText = "Download file type(s) to process")]
  public IEnumerable<DownloadType> Types { get; set; }
}

class ProgramImplementation {
  private readonly IDownloadProcessor _processor;

  public ProgramImplementation(IDownloadProcessor processor) {
    _processor = processor;
  }

  public void Run(string[] args)
  {
    var parser = new Parser(settings => settings.CaseInsensitiveEnumValues = true);
    var result = parser.ParseArguments<Options>(args);
    
    result.WithParsed(options =>
    {
      // convert to flags
      var types = options.Types.Aggregate((i, t) => i | t);

      _processor.Process(types);
    });            
  }
}
```

Much better! We are now accepting multiple download types from the CLI, like this:

    MyApp -t Sales Traffic

This line:

```cs
var parser = new Parser(settings => settings.CaseInsensitiveEnumValues = true);
```

Allows us to pass in case-insensitive enum values like:

    MyApp -t sales traffic

The other magic line:

```cs
var types = options.Types.Aggregate((i, t) => i | t);
```

Converts the array of enum values into a flag. [Thanks StackOverflow](https://stackoverflow.com/questions/21880081/cast-int-array-to-enum-flags)!

> *Note:* The library currently doesn't handle mapping multiple values to a single Enum flags type, so we'll have to make do. No library I could find supports that *and* .NET Core, which I was using to test this code. Time for a PR, maybe?

The `DownloadProcessor` will need to change to accept the new flag enum:

```cs
public void Process(DownloadType types) {
  foreach(var downloader in _downloaders) {
    if (downloader.CanProcess(types)) {
      downloader.Process();
    }
  }
}
```

Finally, that will allow us to simplify our downloaders:

```cs
// SalesDownloader
public bool CanProcess(DownloadType type) {
  return type.HasFlag(DownloadType.Sales);
}

// TrafficDownloader
public bool CanProcess(DownloadType type) {
  return type.HasFlag(DownloadType.Traffic);
}
```

<iframe src="https://giphy.com/embed/tw3tbRjOTqj4s" width="480" height="270" frameBorder="0" class="giphy-embed" allowFullScreen></iframe>

**Do you smell that?** Unlike before, it's the wonderful smell of cleaner code!

This leaves us with a pretty robust implementation with minimal code.

- **It's extensible.** It's easy to add more handlers by adding a new class and enum. The person implementing the new handler will immediately know they require a new enum value when they implement the `IDownloader` interface.
- **It's flexible.** A single handler could handle multiple kinds of downloads.
- **It's easy to reason about.** There's no DI magic that requires additional understanding. We could go so far as to add the [Ninject conventions binding](https://github.com/ninject/Ninject.Extensions.Conventions) extension to eliminate manual registration of downloaders.
- **It's easy to test.** By not relying on Ninject, we now can focus on testing the implementation logic.

## That's All, Folks

You can find the code for this example [on my GitHub](https://github.com/kamranayub/kamranayub.github.io/tree/source/code/2017-07-12-conditional-injection).

There's probably a few things I should mention:

- **IDownloadProcessor interface**: I don't think we need it. There's only a single implementation and it has no complex dependencies, we can easily test it. We could simply remove the interface and bind the concrete implementation or we could axe the class in its entirety and switch to a single function on `ProgramImplementation` if we felt a full object was too much.
- **Is the CLI parser too much?** It could be argued it is. We could just as easily have converted the individual enum values into a flag ourselves. Admittedly, I was hoping the CLI package would do it directly for me but it didn't, I still ended up manually converting it. Still--we get some great benefits: help documentation, validation, and extensibility.
- **You can do all this with Ninject**: But should you? It's always a question you should ask. If you need to *dynamically* find download handlers at runtime from external assemblies or anything more complex than what I showed above, it might be worth the mental overhead. For this case, it's not worth it in my opinion--introduce abstractions when it gets hard to reason about stuff.

I'm positive there are multiple great solutions to this issue. If you had specific ideas, share them in the comments!