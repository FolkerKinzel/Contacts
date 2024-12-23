# FolkerKinzel.Contacts
## Roadmap

### 3.0.0
- [ ] Make the `Contact` class inheritable.

### 2.0.0
- [x] Add an abstract class `MergeableObject<T>`, which has a Method `IsMergeable(T? other)`, and implement it in all classes. 
- [x] Change the overridden `Equals` and `GetHashCode` methods instead to produce compliant results.
- [x] Enable automatic merging of `Contact` objects.
- [x] Implement`Contact: IEnumerable<Contact>`
- [x] Use the System.HashCode structure to produce hash codes.
- [x] End .NET Framework 4.0 support.
- [x] End .NET Framework 4.6.1 support.
- [x] End .NET Framework 4.6.2 support.
- [x] Make `Person.BirthDay` and `Person.Anniversary`  `DateOnly` properties.
