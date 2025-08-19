# Contributing

## Coverage

Running coverage requires some other tools to be installed. The following should guide you through setting up and running coverage:

### Run tests with coverage

```bash
dotnet test --collect:"XPlat Code Coverage"
```

This generates a coverage output in `.coverage` files (binary format) and an XML coverage file (`coverage.cobertura.xml`) by default. The output can be found:

```txt
tests/<project>/TestResults/<guid>/coverage.cobertura.xml
```

### Generate human-readable reports

The raw `coverage.cobertura.xml` is machine readable, but HTML reports are easier to read.

Install the **ReportGenerator** global tool:

```bash
dotnet tool install --global dotnet-reportgenerator-globaltool
```

#### Convert coverage to HTML

To use the tool and convert the coverage files to HTML:

```bash
reportgenerator -reports:tests/**/coverage.cobertura.xml -targetdir:coverage-report -reporttypes:Html
```

Now open:

```bash
coverage-report/index.html
```

> [coverage-report/index.html](coverage-report/index.html)
