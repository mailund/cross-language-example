"Generated"

load(":paket.main.bzl", _main = "main")

def _main_impl(module_ctx):
    _main()
    return module_ctx.extension_metadata(reproducible = True)

paket = module_extension(
    implementation = _main_impl,
)
