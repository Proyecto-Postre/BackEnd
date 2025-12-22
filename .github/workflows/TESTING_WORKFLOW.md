# Documentación del Flujo de Trabajo (Workflow) de CI/CD para .NET

Este documento explica en detalle el funcionamiento del archivo `dotnet-cicd.yml` que se encuentra en esta carpeta. Este flujo de trabajo automatiza la integración continua (CI) para el Backend del proyecto "Dulce Fé".

## 📂 ¿Qué es esta carpeta?
La carpeta `.github/workflows` es donde GitHub busca las instrucciones para **GitHub Actions**. Cada archivo `.yml` aquí representa un "workflow" o flujo de trabajo automatizado.

## 📄 Archivo: `dotnet-cicd.yml`

Este archivo define un robot que se despierta cada vez que hay cambios en el código para asegurar que todo funciona correctamente.

### 1. 🎯 Disparadores (`on`)
Esta sección define **cuándo** se ejecuta el flujo de trabajo.

```yaml
on:
  push:
    branches: [ "main", "master" ]
    paths:
      - 'BackEnd/**'
  pull_request:
    branches: [ "main", "master" ]
    paths:
      - 'BackEnd/**'
```
*   **push**: Se activa cuando subes código directamente (push) a las ramas principales (`main` o `master`).
*   **pull_request**: Se activa cuando alguien abre una solicitud de cambios (Pull Request) hacia las ramas principales.
*   **paths**: Se restringe para ejecutarse **SOLO** si los cambios ocurrieron dentro de la carpeta `BackEnd/`. Si modificas el Frontend o documentación en la raíz, este robot no se molestará en despertar.

### 2. ⚙️ El Trabajo (`jobs`)
Se define un único trabajo llamado `build` que corre en una máquina virtual Linux (`ubuntu-latest`).

```yaml
defaults:
  run:
    working-directory: ./BackEnd
```
*   **working-directory**: Establece que todos los comandos se ejecutarán por defecto dentro de la carpeta `./BackEnd`, para no tener que escribir `cd BackEnd` en cada paso.

### 3. 👣 Los Pasos (`steps`)
Aquí ocurre la magia, paso a paso.

#### Paso 1: Obtener el código
```yaml
- uses: actions/checkout@v4
```
Descarga una copia de tu repositorio en la máquina virtual para poder trabajar con él.

#### Paso 2: Preparar .NET
```yaml
- name: Setup .NET
  uses: actions/setup-dotnet@v4
  with:
    dotnet-version: 9.0.x
```
Instala el entorno de desarrollo de .NET (SDK) versión 9.0 en la máquina virtual. Sin esto, no podríamos compilar C#.

#### Paso 3: Restaurar dependencias
```yaml
- name: Restore dependencies
  run: dotnet restore DulceFe.API/DulceFe.API.csproj
```
Descarga todas las librerías externas (paquetes NuGet) que tu proyecto necesita para funcionar. Es como hacer un `npm install` en JS, pero para .NET.

#### Paso 4: Compilar (`Build`)
```yaml
- name: Build
  run: dotnet build DulceFe.API/DulceFe.API.csproj --no-restore
```
Transforma tu código C# en un ejecutable (binarios).
*   `--no-restore`: Le dice que no intente descargar paquetes de nuevo (porque ya lo hicimos en el paso anterior), lo que ahorra tiempo.
*   Si tienes un error de sintaxis (como un punto y coma faltante), **este paso fallará** y te avisará con un error.

#### Paso 5: Pruebas (`Test`) - 🔍 EL DETALLE QUE PEDISTE
```yaml
- name: Test
  run: dotnet test DulceFe.API.Tests/DulceFe.API.Tests.csproj --no-build --verbosity normal
```
Este es el paso crítico para la calidad. Ejecuta las pruebas automatizadas usando el proyecto `DulceFe.API.Tests`.

*   **`dotnet test`**: Es el comando estándar para correr pruebas en .NET. Busca cualquier método marcado con `[Fact]` o `[Theory]` (usando **xUnit** en tu caso) y lo ejecuta.
*   **`DulceFe.API.Tests/DulceFe.API.Tests.csproj`**: Apunta específicamente a tu proyecto de pruebas.
*   **`--no-build`**: Le dice que no intente compilar de nuevo antes de probar (ya compilamos en el paso anterior).
*   **`--verbosity normal`**: Configura qué tanto detalle quieres ver en los logs. `normal` te muestra qué pruebas pasaron y cuáles fallaron.

**¿Qué pasa aquí?**
1.  El sistema carga tu proyecto de pruebas.
2.  Ejecuta cada prueba individualmente.
3.  Si **TODAS** las pruebas pasan (verde), el paso se marca como exitoso ✅.
4.  Si **AL MENOS UNA** prueba falla (rojo), el paso falla ❌, el flujo de trabajo se detiene y GitHub te envía una notificación de que "rompiste el build".

---
Este archivo es tu red de seguridad. Te permite programar con confianza sabiendo que si cometes un error obvio, el sistema te avisará antes de que llegue a tus usuarios.

## 🛠️ Cómo usarlo y ver resultados

No tienes que hacer nada especial para "activarlo", ya está funcionando.

1.  **Haz tu trabajo normal**: Modifica código, guarda y haz tus commits.
2.  **Sube tus cambios**: Haz `git push` a tu rama.
3.  **Ve a GitHub**: En la página de tu repositorio, haz clic en la pestaña **"Actions"** (arriba).
4.  **Verás el estado**:
    *   🟡 **Amarillo (En progreso)**: El robot está trabajando (compilando y probando).
    *   ✅ **Verde (Éxito)**: Todo salió bien. Tu código es seguro.
    *   ❌ **Rojo (Fallo)**: Algo salió mal. Haz clic para ver los detalles y saber qué prueba falló.
