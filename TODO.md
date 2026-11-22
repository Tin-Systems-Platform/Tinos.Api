# TODO
## Missing Features

- [ ] Add APIs to enumerate disks and retrieve partition information.
- [ ] Expose device paths, types, capacity, sector sizes, and hardware metadata.
- [ ] Expose partition numbers, sizes, offsets, types, flags, and filesystem metadata.
- [ ] Include mount points, mount status, and read-only status where available.
- [ ] Handle missing metadata and unsupported formats gracefully.
- [ ] Provide consistent typed results and error handling.
- [ ] Expose functionality through Lua bindings.
- [ ] Add tests for empty disks, multiple partitions, removable media, and unsupported formats.
- [ ] Ensure all queries are read-only and non-destructive.
- [ ] Lua API registration and documentation for all exposed functions.
- [ ] File IO operations.
- [ ] Custom logging and error handling for Lua bindings.
- [ ] Custom UI library
- [ ] User friendly error messages and logging for Lua bindings.
- [ ] Configuration library for consumers to easily add configuration options and settings.

## Publishing and misc
- [x] Add a NuGet package for the library.
- [x] Make sure it supports VB.NET
- [x] Add a license file and ensure proper licensing for all dependencies.
- [x] Add a README file with usage instructions and examples.
- [ ] Examples of the library on a example cosmos kernel.
