This is a base infrastructure for my projects.
Easy to use, testable and extendable.

Overview of the projects:
Core - Contains the core classes for the infrastructure: Commands, Queries and Events.
Entities - Contains a base entity that can be used with NHibernate.
Entities.FNH - Contains an EntityMap (derived FluentNHibernate ClassMap)
Mvc - Contains a abstract Controller that can be used as a base for other Controllers.
Mvc.TestCore - Contains a TestBase for the AbstractController class, allows for easy testing.
TestCore - Contains the core classes that can be used to create unit tests fast.

Dependencies:
I've tried to keep the amount of dependencies as small as possible for each assembly.
Core - Depends on NHibernate and Microsoft Reactive Extensions
Entities - None
Entities.FNH - Depends on FluentNHibernate, NHibernate and the Entities assembly.
Mvc - Depends on NHibernate, ASP.NET MVC 3 and the Core assembly
Mvc.TestCore - Depends on NHibernate, Moq, and the Core, Mvc and TestCore assemblies.
TestCore - Depends on FluentNHibernate, NHibernate and SQLite.

Depends on the following projects:
* NHibernate
* FluentNHibernate
* Moq
* Microsoft Reactive Extensions
* SQLite 1.0.66