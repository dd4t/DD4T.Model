[![AppVeyor](https://ci.appveyor.com/api/projects/status/github/dd4t/DD4T.Model?branch=master&svg=true&passingText=master)](https://ci.appveyor.com/project/DD4T/dd4t-model)

[![AppVeyor](https://ci.appveyor.com/api/projects/status/github/dd4t/DD4T.Model?branch=develop&svg=true&passingText=develop)](https://ci.appveyor.com/project/DD4T/dd4t-model)
# DD4T.Model
DD4T content model (.NET)
Contains the model classes used in DD4T templates as well as the DD4T presentation layer for .NET.


## Release notes for version 2.6

- Issue 44: Fixed bug with deserialization of keywords (see https://tridion.stackexchange.com/questions/21828/upgrade-to-tridion-9-5-and-net-dd4t-2-5-producing-error-with-ikeyword-on-dynami)
- Issue 47: Introduced setting DD4T.JsonSerializerMaxDepth to specify the maximum depth for NewtonSoft (see https://stackoverflow.com/questions/68576787/newtonsoft-getting-the-readers-maxdepth-of-64-has-been-exceeded-error-while)
- Upgraded to NewtonSoft 13.0.1

Note: if, after upgrading to this version of DD4T, you encounter the error 'Newtonsoft: Getting "The reader's MaxDepth of 64 has been exceeded" error while parse json string into object', you can fix it by adding an appSetting to your
Web.config with the key DD4T.JsonSerializerMaxDepth and a higher value than 64. The required value depends on the depth of your content. Component links, embedded fields and metadata can all add to the depth.


## Release notes for version 2.5.1

Models are now serializable so they can be persisted in a distributed cache, on a file system, are any other medium that requires serialization.


## Release notes for version 2.5

- Added support for regions (introduced in Tridion 9)
- Upgraded Newtonsoft.Json to 11.0.2
