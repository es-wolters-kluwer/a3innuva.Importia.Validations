# a3innuva.Importia.Validations

Herramienta de consola (.NET 10) para validar ficheros de migración de Importia (`IMigrationSet`) sin necesidad de importarlos en la plataforma.

## ¿Para qué sirve?

Lee un fichero de migración en formato JSON (o un `.zip` que lo contenga), lo deserializa con el SDK de Importia y ejecuta sus validaciones. Muestra por consola:

- Si el *migration set* es válido.
- Si la información de migración (`Info`) es válida.
- Si todas las entidades son válidas y, en caso contrario, la lista de errores con su línea, el id de la entidad y el código.

## Requisitos

- [.NET SDK 10](https://dotnet.microsoft.com/download)
- Acceso al feed de NuGet donde se publican los paquetes `a3innuva.Importia.SDK.Extensions` y `a3innuva.Importia.SDK.Serialization`.

## Uso

```powershell
dotnet run --project a3innuva.Importia.Validations.Console -- <ruta-al-fichero>
```

`<ruta-al-fichero>` puede ser:

- Un fichero `.json` con el migration set.
- Un fichero `.zip`: se valida el primer fichero que contenga.

Ejemplo:

```powershell
dotnet run --project a3innuva.Importia.Validations.Console -- C:\Innuva\D95753067_2020_Expedidas_2020_001.zip
```

También se puede compilar y ejecutar el binario directamente:

```powershell
dotnet build -c Release
a3innuva.Importia.Validations.Console\bin\Release\net10.0\a3innuva.Importia.Validations.Console.exe <ruta-al-fichero>
```

## Ejemplo de salida

```
Fichero: C:\Innuva\D95753067_2020_Expedidas_2020_001.zip
Migration set es válido: False
Migration info es válido: True
Todas las entidades son válidas: False
Línea : 12 - Id: 3f2b8c1e-0000-0000-0000-000000000000 - <código de error>
```

Si el fichero no se puede leer o deserializar, se muestra el mensaje de la excepción.

## Estructura

- `a3innuva.Importia.Validations.Console`: aplicación de consola (`Program.cs`).
