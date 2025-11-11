# PP4 – BooksApp

## Datos del estudiante
Nombre: Ronal Delgado Vásquez  
Carné: FI20016028

---

## Comandos utilizados (CLI)

```bash
dotnet new sln -n BooksApp
dotnet new console -n BooksApp -f net8.0
dotnet sln BooksApp.sln add BooksApp/BooksApp.csproj
cd BooksApp
dotnet add package Microsoft.EntityFrameworkCore --version 9.0.0
dotnet add package Microsoft.EntityFrameworkCore.Sqlite --version 9.0.0
dotnet add package Microsoft.EntityFrameworkCore.Design --version 9.0.0
dotnet add package CsvHelper --version 30.0.1
dotnet build
dotnet ef migrations add InitialCreate
dotnet ef database update
dotnet run     
dotnet run     
```

---

## Páginas web consultadas
* https://stackoverflow.com/questions/59375668/how-to-install-nuget-package-using-windows-command-line-in-c-sharp-project
* https://stackoverflow.com/questions/41536603/visual-studio-code-entity-framework-core-add-migration-not-recognized

---

## Prompts utilizados CHATGPT
1- Cuando ejecuto `dotnet ef` me dice que no existe el comando. Ya instalé .NET 8. ¿Qué hago?
R/ Eso ocurre cuando `dotnet-ef` no está instalado como herramienta global. Ejecuta:  
`dotnet tool install --global dotnet-ef`  
Si falla, limpia las fuentes de NuGet con:  
`dotnet nuget locals all --clear`.  
Luego cerrá la terminal, volvé a abrir y probá de nuevo `dotnet ef`.  
Si sigue sin funcionar, usá `dotnet new tool-manifest` y luego  
`dotnet tool install dotnet-ef --version 9.0.0` para instalarlo localmente.

2- Cuando hago `dotnet ef migrations add InitialCreate` sí me compila, pero cuando hago `database update` me dice que no se encuentra el comando.
R/ El error ocurre porque aún estás usando una terminal donde el PATH no se actualizó.  
Solución:  
1. Cerrá la terminal de VS Code.  
2. Abrí una nueva terminal.  
3. Probá: `dotnet ef`.  
Si ya funciona, entonces ejecutá:  
`dotnet ef migrations add InitialCreate`  
`dotnet ef database update`.

---

## Preguntas teóricas

### 1) ¿Cómo resultaría usar Code First con una base de datos NoSQL como MongoDB? ¿Y con Database First? ¿Habría complicaciones con las Foreign Keys?

En bases de datos NoSQL como MongoDB no existen claves primarias y foráneas como restricciones formales.  
Con Code First, las relaciones entre entidades tendrían que manejarse a nivel de la aplicación. Las migraciones serían lógicas y no estructurales como en SQL. Con Database First, no es viable porque MongoDB no tiene un esquema que permita generar entidades automáticamente. No tiene tablas, PK, FK para que una herramienta pueda invertir el diseño y crear un modelo.

Sí habría complicaciones con las FK, porque en NoSQL no se garantiza su existencia ni su consistencia. Todo recaería en el código de la aplicación.

---

### 2) ¿Qué otro carácter podría usarse como separador de valores? ¿Qué extensión tendría el archivo?

Se puede usar el punto y coma `;`como separador.  Una extensión apropiada sería: `.ssv`

Este separador es útil porque: Evita conflictos cuando los valores contienen comas, es fácil de procesar en la mayoría de lenguajes y es compatible con editores.