\# PP1 – Suma de números naturales



\*\*Nombre:\*\* Ronal Delgado Vásquez  

\*\*Carné:\*\* FI20016028  



---



\## Comandos dotnet utilizados (CLI)



```bash

dotnet --info

dotnet new sln -n PP1

dotnet new console -n SumaNaturales -f net8.0

dotnet sln PP1.sln add SumaNaturales/SumaNaturales.csproj

dotnet build PP1/PP1.sln

dotnet run --project PP1/SumaNaturales/SumaNaturales.csproj



\## Páginas web consultadas



* https://stackoverflow.com/questions/38506858/signed-integer-overflow-behaviour/38507066#38507066
* https://stackoverflow.com/questions/63026002/overflow-exception-during-sum-of-all-the-values-of-integer-array-in-c-sharp?utm\_source=chatgpt.com
* https://es.stackoverflow.com/questions/392347/resuelto-c-funci%C3%B3n-recursiva-que-permita-obtener-todos-los-n%C3%BAmeros-primos-inf



\## Prompts de IA utilizados



* Como reiniciar powershell si se queda pegado?
* Como ves el código asi? Esta optimo o necesita mejoras?
* Esto cumple con las indicaciones que pide el profe?



\## Preguntas

1\. ¿Por qué todos los valores resultantes tanto de n como de sum difieren entre métodos (fórmula e implementación iterativa) y estrategias (ascendente y descendente)?



Porque los calculos se hacen en tipo int de 32 bits. Cuando se pasa ocurre overflow y los resultados cambian de signo o se reinician. En la formula el overflow aparece antes porque multiplica primero, y en la iterativa aparece despues porque se va acumulando. La diferencia entre ascendente y descendente es que uno busca el ultimo valor valido y el otro el primero valido.



2\. ¿Qué cree que sucedería si se utilizan las mismas estrategias (ascendente y descendente) pero con el método recursivo de suma (SumRec)?



Con valores pequeños funciona, pero al crecer un StackOverflowException porque cada llamada recursiva ocupa un espacio. Por eso no se puede usar para valores grandes

