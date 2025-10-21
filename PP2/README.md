# PP2 – Calculadora Binaria 

##**Nombre** Ronal Delgado Vásquez  
##**Carné:** FI20016028

## Comandos que usé

```bash
mkdir PP2
cd PP2
dotnet new sln -n PP2

dotnet new mvc -n BinaryCalc -f net8.0

dotnet sln PP2.sln add BinaryCalc/BinaryCalc.csproj

dotnet run --project BinaryCalc 

## Páginas web consultadas

* https://learn.microsoft.com/en-us/aspnet/core/tutorials/first-mvc-app/validation?view=aspnetcore-9.0&utm_source=chatgpt.com
* https://www.alwaysdeveloping.net/dailydrop/2022/06/20-base-conversion/?utm_source=chatgpt.com

## Prompts de IA utilizados

* Que hacer si no me da direccion local?
 R/ Cuando corrés el proyecto con dotnet run, debería aparecerte una línea con algo como Now listening on: https://localhost:xxxx. Si no te da dirección local, normalmente es porque el certificado HTTPS de desarrollo no está instalado. La solución es ejecutar:

dotnet dev-certs https --trust


y aceptar el certificado. Luego volvés a correr dotnet run --project BinaryCalc y ya debería mostrarte la dirección en localhost.

* Cumple con las indicaciones que pide el profe? 
R/ Sí, cumple con lo que pidió. La aplicación está hecha en ASP.NET Core MVC con .NET 8, tiene un formulario con dos cadenas binarias (a y b), validaciones con Data Annotations, operaciones binarias (AND, OR, XOR) y aritméticas (suma y multiplicación), y los resultados se muestran en una tabla con todas las bases (bin, oct, dec, hex). Además, a y b se muestran con 8 dígitos rellenando con ceros a la izquierda, tal como se especificaba.

* me da Warnings VSCode, exactamente cual es el error? 
R/ No son errores, son warnings de C#. Básicamente dicen que algunas propiedades (A, B, Label, etc.) pueden ser null porque no tienen un valor inicial en el constructor. Se resuelve asignándoles un valor por defecto, por ejemplo = string.Empty;. Con eso desaparecen los warnings. Lo importante es que el build dice Build succeeded, o sea, el proyecto compila y funciona bien.

* como cambio esos warnings para que no salgan y funcione todo?
R/ Los warnings salen porque el método IsValid no coincide con la firma esperada y porque las propiedades string del modelo pueden quedar sin inicializar. Para arreglarlo:

En BinaryStringAttribute, cambia la firma a: protected override ValidationResult IsValid(object? value, ValidationContext validationContext)


## Preguntas

* ¿Cuál es el número que resulta al multiplicar, si se introducen los valores máximos permitidos en a y b? Indíquelo en todas las bases (binaria, octal, decimal y hexadecimal).

En este caso si se multiplican a= 11111111 y b= 11111111 daria: 
Resultado en decimal: 65025
En binario: 1111111000000001
En octal: 177001
En hex: FE01

* ¿Es posible hacer las operaciones en otra capa? Si sí, ¿en cuál sería?
Diria que si, segun unas busquedas en internet y consultas, se podria pasar la logica a un servicio(como BinaryService) en vez de hacer todo en el Controller

