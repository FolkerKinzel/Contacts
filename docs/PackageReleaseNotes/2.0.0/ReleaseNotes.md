# FolkerKinzel.Contacts 2.0.0
## Package Release Notes

The behavior of `Equals` has changed in all classes. This is a breaking change that requires a new major version.

Up to and including version 1.5.0, the `Equals` methods tried to determine whether the object to be compared describes
 an equal identity. This is different from the systematic comparison of all fields. Unfortunately, this type of 
comparison prevented the rule that `GetHashCode` should return the same values for equal objects.

In order to fix this issue, beginning from version 2.0.0 all classes inherit from the abstract base class `MergableObject<T>`
that has a an instance method IsMergeable(T?) and a static method AreMergeable(T?, T?), which have the same 
functionality that was provided by the `Equals` methods up to version 1.5.0.

The `Equals` methods now perform a strict comparison of all fields, and `GetHashCode` now provides compliant results.

The `Mergable<T>` class comes with an abstract method `Merge`, which allows the automatic merging of the data of two 
instances.

.

- [Version History](https://github.com/FolkerKinzel/Contacts/releases)
