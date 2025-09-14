load("@rules_dotnet//dotnet:defs.bzl", _csharp_test = "csharp_test")

def csharp_xunit_test(
        name,
        srcs,
        deps = [],
        data = [],
        target_frameworks = ["net9.0"],
        visibility = None,
        **kwargs):
    _csharp_test(
        name = name,
        srcs = srcs + ["//tools:xunit_main.cs"],
        deps = deps + [
            "@paket.main//xunit.v3.core:xunit.v3.core",
            "@paket.main//xunit.v3.assert:xunit.v3.assert",
            "@paket.main//xunit.v3.common:xunit.v3.common",
            "@paket.main//xunit.v3.extensibility.core:xunit.v3.extensibility.core",
            "@paket.main//xunit.v3.runner.inproc.console:xunit.v3.runner.inproc.console",
            "@paket.main//xunit.v3:xunit.v3",
            "@paket.main//shouldly:shouldly",
        ],
        data = data,
        target_frameworks = target_frameworks,
        visibility = visibility,
        **kwargs
    )
    