\# PP3



\##\*\*Nombre\*\* Ronal Delgado Vásquez  

\##\*\*Carné:\*\* FI20016028



\## Comandos que usé

```bash

mkdir PP3 \&\& cd PP3

dotnet new sln -n PP3

dotnet new webapi -n PP3.Api -f net8.0 --no-https

dotnet sln PP3.sln add PP3.Api/PP3.Api.csproj

dotnet run --project PP3.Api

cd PP3/PP3.Api

dotnet run



\## Páginas web consultadas



\* https://stackoverflow.com/questions/tagged/asp.net-minimal-apis?utm\_source=chatgpt.com
\* https://www.freecodecamp.org/news/create-a-minimal-api-in-net-core-handbook/?utm\_source=chatgpt.com



\## Prompts de IA utilizados



\* Cómo configuro que el endpoint GET / redirija al Swagger en el API?

R/ Puedes usar app.MapGet("/", () => Results.Redirect("/swagger")). Esto crea un endpoint GET en la ruta / que hace un redireccionamiento HTTP hacia /swagger, que es la interfaz UI de Swagger. Así cumples la funcionalidad: el root redirige al Swagger.

\* Cómo hago para que el parámetro de xml en los headers cambie entre JSON y XML?

R/ En la definición del endpoint, puedes usar \[FromHeader(Name = "xml")] bool? xml. Luego, en el código, chequeas si xml == true, y en ese caso serializas a XML (usando XmlSerializer y un StringWriterWithEncoding(Encoding.Unicode)) y devuelves Results.Text(xml, "application/xml; charset=utf-16"). Si no, devolvemos JSON con Results.Json(...).

\* Tengo el error HTTP 500  al enviar un formulario POST, por qué pasa esto?

R/ En .NET Core/Minimal API, por defecto puede estar habilitado Anti-Forgery (CSRF) cuando usas FromForm. Si no tienes configurado un middleware para Anti-Forgery, obtienes un error. La solución es para API sin formularios web usar .DisableAntiforgery() en el MapPost/MapPut/MapDelete para desactivar esa verificación.



\## Preguntas



\* ¿Es posible enviar valores en el Body (por ejemplo, en el Form) del Request de tipo GET?

R/ Es posible si, pero no es lo recomendado. Segun busqué, el protocolo HTTP no prohíbe tener un body en un GET, pero la mayoría de navegadores y servidores lo ignoran, pero técnicamente si se puede. 

\* ¿Qué ventajas y desventajas observa con el Minimal API si se compara con la opción de utilizar Controllers?

R/ 
Ventajas: Es mas rápido para configurar, tiene menos código y es ideal para las APIs pequeñas, ya que todo se hace desde el mismo archivo y es relativamente fácil de entender. 

Desventajas: Cuando el proyecto se hace mas grande se vuelve un poco difícil de organizar ya que no trae herramientas como los Controllers y para trabajos más grandes es mejor usar MVC.

