Title: "JavaScript heap out of memory with WebdriverIO"
Published: false
Lead: I recently spent a few hours wrestling with a JavaScript out-of-memory heap error when running WebdriverIO in a continuous integration environment
Tags:
- WebdriverIO
- JavaScript
- Testing
---

Throughout working on a course you usually run into bugs and issues that throw you for a loop for awhile. In this case, it was doubly frustrating because I had previously set up [webdriverio](https://webdriver.io) to run in my continuous integration environment (GitHub Actions) and _it was working fine_. Until it stopped working.

I kept getting this error:

```
<--- Last few GCs --->

[168:0x5ba0970]   105925 ms: Mark-sweep (reduce) 2046.6 (2050.9) -> 2045.7 (2050.9) MB, 588.4 / 0.0 ms  (average mu = 0.053, current mu = 0.021) allocation failure scavenge might not succeed

[0-0] 
<--- JS stacktrace --->

[0-0] FATAL ERROR: Ineffective mark-compacts near heap limit Allocation failed - JavaScript heap out of memory
[0-0]  1: 0xa3ac30 node::Abort() [/usr/local/bin/node]
[0-0]  2: 0x98a45d node::FatalError(char const*, char const*) [/usr/local/bin/node]
[0-0]  3: 0xbae25e v8::Utils::ReportOOMFailure(v8::internal::Isolate*, char const*, bool) [/usr/local/bin/node]
[0-0]  4: 0xbae5d7 v8::internal::V8::FatalProcessOutOfMemory(v8::internal::Isolate*, char const*, bool) [/usr/local/bin/node]
[0-0]  5: 0xd56125  [/usr/local/bin/node]
[0-0]  6: 0xd56acb v8::internal::Heap::RecomputeLimits(v8::internal::GarbageCollector) [/usr/local/bin/node]
[0-0]  7: 0xd6481c v8::internal::Heap::PerformGarbageCollection(v8::internal::GarbageCollector, v8::GCCallbackFlags) [/usr/local/bin/node]
[0-0]  8: 0xd65684 v8::internal::Heap::CollectGarbage(v8::internal::AllocationSpace, v8::internal::GarbageCollectionReason, v8::GCCallbackFlags) [/usr/local/bin/node]
[0-0]  9: 0xd680fc v8::internal::Heap::AllocateRawWithRetryOrFailSlowPath(int, v8::internal::AllocationType, v8::internal::AllocationOrigin, v8::internal::AllocationAlignment) [/usr/local/bin/node]
[0-0] 10: 0xd2f3aa v8::internal::Factory::AllocateRaw(int, v8::internal::AllocationType, v8::internal::AllocationAlignment) [/usr/local/bin/node]
[0-0] 11: 0xd29254 v8::internal::FactoryBase<v8::internal::Factory>::AllocateRawWithImmortalMap(int, v8::internal::AllocationType, v8::internal::Map, v8::internal::AllocationAlignment) [/usr/local/bin/node]
[0-0] 12: 0xd2a789 v8::internal::FactoryBase<v8::internal::Factory>::NewStruct(v8::internal::InstanceType, v8::internal::AllocationType) [/usr/local/bin/node]
[0-0] 13: 0xd36be6 v8::internal::Factory::NewStackTraceFrame(v8::internal::Handle<v8::internal::FrameArray>, int) [/usr/local/bin/node]
[0-0] 14: 0xc28a98  [/usr/local/bin/node]
[0-0] 15: 0xc2fb66 v8::internal::Builtin_CallSitePrototypeToString(int, unsigned long*, v8::internal::Isolate*) [/usr/local/bin/node]
[0-0] 16: 0x13f5159  [/usr/local/bin/node]
```

What was **weird** is that the tests all passed!

It did not turn out to be a Selenium Grid issue, as I wasn't using that but it turned out to be another simple fix (i.e. nothing to do with memory size).

**I was missing the `@wdio/sync` package from my package dependencies after I split out my e2e tests into a separate folder.**

Once I added `@wdio/sync` back, things worked. The telltale sign was that the tests were taking 1 minute when before I checked the logs and the tests used to run in 4 seconds. **Ding, ding, ding!** It must not wait properly for the commands without the sync package and uses up more and more memory.
<!--stackedit_data:
eyJoaXN0b3J5IjpbLTE3NDQ5MTgxMDJdfQ==
-->