# Container Image for Hello Application

This directory builds a container image for the `hello` application using Bazel's native OCI support.

## Building and Running

```bash
# Build the container image
bazel build //docker:hello_image

# Load into Docker (this runs the load script)
bazel run //docker:hello_docker_load

# Run the container
docker run hello:latest
```

### Alternative: Manual tarball loading

If you need the actual tarball file:

```bash
# Build with tarball output group
bazel build //docker:hello_docker_load --output_groups=+tarball

# Load the tarball into Docker
docker load < bazel-bin/docker/hello_docker_load/tarball.tar

# Run the container  
docker run hello:latest
```

## What's Inside

The container includes:

- The `hello` C# application (at `/app/hello`)
- The `greetings_rust` shared library (in `/app/`)
- .NET 9.0 runtime environment

## Available Targets

- `//docker:hello_image` - The OCI image
- `//docker:hello_docker_load` - Docker load script

## How It Works

The C# application calls into Rust code via P/Invoke, and both components are packaged together in the container using Bazel's OCI rules for reproducible, cacheable builds.
