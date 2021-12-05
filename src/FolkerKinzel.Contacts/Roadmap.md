# FolkerKinzel.Contacts
## Roadmap

.

### 2.0.0
- [ ] Add an interface IIdentityComparer, which has a Method `IsProbablyTheSame(object? other)`, and implement it in all classes. Change
the overriden `Equals` and `GetHashCode` methods instead to produce compliant results.

### 2.1.0
- [ ] Add dynamic properties to the Contact class.

### 3.0.0
- [ ] End .NET Framework 4.0 support.
- [ ] Make `Person.BirthDay` and `Person.Anniversary`  `DateOnly` properties.