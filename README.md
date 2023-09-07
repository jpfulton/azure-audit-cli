# azure-audit-cli

[![ci](https://github.com/jpfulton/azure-audit-cli/actions/workflows/ci.yml/badge.svg)](https://github.com/jpfulton/azure-audit-cli/actions/workflows/ci.yml)
![License](https://img.shields.io/badge/License-MIT-blue)
![Visitors](https://visitor-badge.laobi.icu/badge?page_id=jpfulton.azure-audit-cli)

## Installation

You can install this tool globally, using the dotnet tool command:

```bash
dotnet tool install --global azure-audit-cli 
```

## Upgrading

When there is a new version available on NuGet, you can use the `dotnet tool update` command to upgrade:

```bash
dotnet tool update --global azure-audit-cli 
```

With a `--version` parameter, you can specify a specific version to install. Use the `--no-cache` parameter to force a re-download of the package if it cannot find the latest version.

## Usage

You can invoke the tool using the `azure-audit` command. You can use the `--help` parameter to get a list of all available options.

```bash
azure-audit --help
```