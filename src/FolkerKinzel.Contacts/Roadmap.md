# FolkerKinzel.Contacts
## Roadmap

### 2.0.0
- [x] Add an abstract class `MergeableObject<T>`, which has a Method `IsMergeable(T? other)`, and implement it in all classes. 
- [x] Change the overridden `Equals` and `GetHashCode` methods instead to produce compliant results.
- [x] Enable automatic merging of `Contact` objects.

### 2.1.0
- [ ] Make the `Contact` class inheritable.

### 3.0.0
- [ ] End .NET Framework 4.0 support.
- [ ] Make `Person.BirthDay` and `Person.Anniversary`  `DateOnly` properties.
- [ ] Use the `HashCode` structure to produce hash codes.