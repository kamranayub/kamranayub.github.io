---
layout: post
title: "Using the TransitioningContentControl with Caliburn for In-Page Transitions"
published: false
comments: true
categories:
permalink: /blog/posts/63/using-the-transitioningcontentcontrol-with-calibur
---

I've been working on a Windows Phone app, so expect to see an influx of posts as I find time to write and record my experiences developing an app. It's been a learning experience, coming primarily from a web background. Once upon a time, I did write a Blackjack app in WPF but that was awhile ago. Still, most of what I know of XAML came from WPF.

### The TCC

I still don't exactly know the details but once upon a time in the Silverlight Toolkit for Windows Phone, there was a control in there called the `TransitioningContentControl`. It's purpose was to help with page transitions, but it was an actual control so you could put it into a XAML page and animate content within it.

Somewhere along the line, this was lost (possibly in favor of the `TransitionFrame`). Old posts referenced this control and I couldn't find anywhere where there was a working, compatible WP7.1 version. A helpful person on StackOverflow pointed me to this post on the topic. In it, the author brought the code into his project and made it work. It wasn't the best solution but it compiled and worked, so I'm not complaining.

To get started, you should open that project and bring in the relevant source code for the TCC, including the App.xaml styles for the different transitions.

### Modding the Transitions

I specifically wanted a slide transition, since I'm making a 3 step "wizard". Unfortunately, the source included in that sample doesn't include a slide transition... but luckily for us, the latest Windows Phone Toolkit does! And since the TCC was an offspring of the original SL Toolkit for WP, the XAML styles are actually pretty similar. You really only need to modify a few pieces to bring in the transition. For your benefit, here are the new Visual States you'll need to add into your resource file (App.xaml in this case):

<script src="https://gist.github.com/4710775.js"></script>

These go inside of the main `TransitioningContentControl` storyboard "PresentationStates".

## Using Caliburn

The next step is to actually use the control in a page to host views via Caliburn. To do this, we'll create a OneView conductor:

```c#
public class TCCViewModel : Conductor<Screen>.Collection.OneActive {
    
}
```

And it's view:

```xml
<phone:PhoneApplicationPage
    x:Class="Demo.Views.TCCView"
    xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
    xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
    xmlns:phone="clr-namespace:Microsoft.Phone.Controls;assembly=Microsoft.Phone"
    xmlns:shell="clr-namespace:Microsoft.Phone.Shell;assembly=Microsoft.Phone"
    xmlns:d="http://schemas.microsoft.com/expression/blend/2008"
    xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006"
    xmlns:viewModels="clr-namespace:Demo.ViewModels"    
    xmlns:cal="clr-namespace:Caliburn.Micro;assembly=Caliburn.Micro"    
    xmlns:controls="clr-namespace:Demo.Controls"
    FontFamily="{StaticResource PhoneFontFamilyNormal}"
    FontSize="{StaticResource PhoneFontSizeNormal}"
    Foreground="{StaticResource PhoneForegroundBrush}"
    d:DataContext="{d:DesignInstance IsDesignTimeCreatable=True, Type=viewModels:TCCViewModel}"
    cal:Bind.AtDesignTime="True"    
    SupportedOrientations="Portrait" Orientation="Portrait"
    mc:Ignorable="d"
    shell:SystemTray.IsVisible="True">

    <Grid x:Name="LayoutRoot" Background="Transparent">
        <Grid.RowDefinitions>
            <RowDefinition Height="Auto"/>
            <RowDefinition Height="*"/>
        </Grid.RowDefinitions>

        <!--TitlePanel contains the name of the application and page title-->
        <StackPanel Grid.Row="0" Margin="12,17,0,28">
            <TextBlock Text="{Binding DisplayName}" Style="{StaticResource PhoneTitleStyle3}" Margin="9,0,0,0"/>            
        </StackPanel>

        <!--ContentPanel - place additional content here-->
        <Grid x:Name="ContentPanel" Grid.Row="1" Margin="12,0,12,0">            
            <controls:TransitioningContentControl 
                x:Name="ActiveItem" 
                Transition="NextTransition" 
                Style="{StaticResource TransitioningStyle}"/>            
        </Grid>
    </Grid>

</phone>
```

A few things to note:

* The TCC will be bound to the "ActiveItem" property of our view model
* The default transition will be the "next" transition
* We point to the styles defined in our App.xaml
* I'm using the neat Design-Time binding feature of Caliburn, though for this screen it doesn't work. Still, I kept it because it's essential to Caliburn-based development.

### Hooking up the VM

Now we need to do a few things:

* Add some screens
* Transition between them properly (previous / next)
* Prevent navigation during a transition (you'll run into out-of-sync issues otherwise!)
* Prevent children from closing if we're transitioning

Let's create some children of our conductor. I'm using the nice shadowing feature of C# to strongly-type my parent view which Caliburn sets automatically:

```c#
public class ChildViewModel : Screen, IChild<TCCViewModel> {

    public new TCCViewModel Parent {
        get { return base.Parent as TCCViewModel; }
        set { base.Parent = value; }
    }

}
```

We have one screen here and what I'm not showing is their corresponding UserControls. I will assume you know how to make that! I'm also choosing to use a single view model for simplicity's sake. You can use any number of view models.

Now let's add them to our screen collection:

```c#
public class TCCViewModel : Conductor<Screen>.Collection.OneActive {

    public TCCViewModel(
        Func<ChildViewModel> createChild) {
        _createChild = createChild;
    }

    private Func<ChildViewModel> _createChild;

    protected override void OnInitialize() {
        base.OnInitialize();

        // Add children
        Items.Add(_createChild());
        Items.Add(_createChild());
        Items.Add(_createChild());

        // Show screen 1 at first
        ActivateItem(Items[0]);
    }
}
```

A few things to note here:

* In the constructor, we're taking advantage of Caliburn's view model dependency injection. It helpfully provides a function you can call that creates an instance of the view model you specify. This is great because you don't need to manually pass in dependencies when you create your children; Caliburn will handle DI for you.
* We create our screens and add them to our `Items` collection and activate the first screen. If you, like me, want a dynamic set of screens, you don't need to add all the screens right away. You can add them any time, so you can create a dynamic set of views. You can also create any *kind* of screen, I chose to reuse the same one for demo's sake. In my app, I have a dynamic set of screens I display based on user input.

### Handling Transitions

So far, so good. We have some screens to display in our TCC container. Now let's add some (forward) navigation methods:

```c#
// TCCViewModel

    public void GoToNextScreen() {
        var nextIndex = Items.IndexOf(ActiveItem) + 1;

        // Already on last screen?
        if (nextIndex >= Items.Length) return;

        ActivateItem(Items[nextIndex]);
    }

    public void GoToPreviousScreen() {
        var prevIndex = Items.IndexOf(ActiveItem) - 1;

        // Already on first screen?
        if (prevIdex <= 0) return;

        ActivateItem(Items[prevIndex]);
    }
```

The `GoToNextScreen` function will be bound to a button on the child view while the previous method will get called when the user hits the back button (getting to that!).

Let's hookup a button in the child view to our method:

```xml
<Button x:Name="GoToNextScreen" FontSize="26">Go to Next Screen</Button>
```

Caliburn will walk the hierarchy to discover the right method to call, so while we could add a method in our child view model to handle this if we wished, it's just easier to let the "master" handle it (and it's a good separation).

So, if you start your sample now, you should see a working, sliding content control! Now hit the back button. *Whoops!* If you had an entry page into your app, you probably went back to there otherwise nothing happened... it didn't slide back to the previous screen(s).

Let's handle back button navigation. To do so, we will hook into our view using Caliburn's robust message API:

```xml
<phone:PhoneApplicationPage
    ... snip

    cal:Message.Attach="[Event BackKeyPress] = [HandleBackKeyPress($args)]">
```

Here we tell Caliburn to handle the BackKeyPress event and pass in the event arguments for us to handle.

Now let's add a handler to our view model, let's refactor a bit to make this easier:

```c#
    public bool CanGoToNextScreen {
        get {
            var nextIndex = Items.IndexOf(ActiveItem) + 1;

            // Already on last screen?
            return nextIndex < Items.Length;
        }
    }

    public bool CanGoToPreviousScreen {
        get {
            var prevIndex = Items.IndexOf(ActiveItem) - 1;

            // Already on first screen?
            return prevIdex > 0;
        }
    }

    protected override void ActivateItem(object screen) {
        base.ActivateItem(screen);

        // Notify bindings in view
        NotifyOfPropertyChanged(() => CanGoToNextScreen);
        NotifyOfPropertyChanged(() => CanGoToPreviousScreen);
    }

    public void GoToNextScreen() {
        if (!CanGoToNextScreen) return;

        ActivateItem(Items[nextIndex]);
    }

    public void GoToPreviousScreen() {        
        if (!CanGoToPreviousScreen) return;
        
        ActivateItem(Items[prevIndex]);
    }

    public void HandleBackKeyPress(BackKeyPressEventArgs e) {
        if (CanGoToPreviousScreen) {

            // Cancel navigation
            e.Cancel = true;

            // Go to previous screen
            GoToPreviousScreen();
        }
    }
```

Some things to note:

* I moved the logic 