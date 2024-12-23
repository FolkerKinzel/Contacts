- .NET Framework 4.0 support and .NET Framework 4.6.1 support has been removed.
- .NET 8.0 and .NET Framework 4.6.2 support has been removed.
- The behavior of `Equals` has changed in all classes. This is a breaking change that requires a new major version.

>Up to and including version 1.5.0, the `Equals` methods tried to determine whether the object to be compared describes
 an equal identity. This is different from the systematic comparison of all fields. Unfortunately, this type of 
comparison prevented the rule that `GetHashCode` should return the same values for equal objects.

>In order to fix this issue, beginning from version 2.0.0 all classes inherit from the abstract base class `MergableObject<T>`
that has a an instance method `IsMergeableWith(T?)` and a static method `AreMergeable(T?, T?)`, which have the same 
functionality that was provided by the `Equals` methods up to version 1.5.0.

>The `Equals` methods now perform a strict comparison of all fields, and `GetHashCode` now provides compliant results.

- The `Mergable<T>` class comes with an abstract `Merge` method, which allows the automatic merging of the data of two 
instances.

- The `Contact` class implements `IEnumerable<Contact>` now.
- `Person.BirthDay` and `Person.Anniversary` are `DateOnly` properties now.
- `Contact.TimeStamp` is `DateTimeOffset` now.
- The `System.HashCode` structure is used to produce hash codes.

&nbsp;
>**Project reference:** On some systems, the content of the CHM file in the Assets is blocked. Before opening the file right click on the file icon, select Properties, and **check the "Allow" checkbox** - if it is present - in the lower right corner of the General tab in the Properties dialog.
