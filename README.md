# 🐉 Abyssal Flight - Prototipo de Videojuego 2D
**Desarrollado para:** Programación de Videojuegos | Universidad Santo Tomás.

## 📝 Descripción
Un arcade de scroll vertical con estética **Grimdark**. El jugador controla un dragón en un descenso infinito por un abismo gótico, gestionando energía y esquivando obstáculos para sobrevivir.

🚀 Características Técnicas (Cumplimiento de Rúbrica)
1. Arquitectura y Modularidad
Código Modular: Sistema basado en gestores independientes (MenuManager, ScoreManager, GameOverManager) eliminando el "código spaghetti".

Uso de Prefabs: Todos los elementos repetitivos están consolidados como Prefabs finales, optimizando la escalabilidad visual.

Gestión de Datos: Implementación de persistencia local mediante PlayerPrefs para la tabla de High Scores.

2. Flujo de Menús y UI Dinámica
Transiciones Inmersivas: Sistema de video integrado como transición para eliminar tiempos de carga, manteniendo la coherencia estética.

Canvas Dinámico: Interfaz de usuario (UI) totalmente adaptativa y legible en diversas resoluciones mediante Anchors y TextMeshPro.

Menús Completos: Flujo integrado de Inicio → Transición de Video → Tabla de Puntuaciones → Game Over con captura de usuario.

3. Sistema de Animación y Sonido
Animator Controller: Máquina de estados (FSM) implementada para transiciones fluidas de animaciones mediante Triggers y Floats.

Gestión de Audio: Implementación de BGM (Música de fondo) con Fade Out automático y SFX (efectos de sonido) disparados por eventos lógicos del juego.

4. Físicas y Sensores
Lógica estable: Movimiento basado en FixedUpdate y componentes de físicas (Rigidbody2D) para evitar comportamientos erráticos.

Eventos de Colisión: Uso de OnTriggerEnter para la detección de sensores en el mundo, permitiendo mecánicas interactivas de forma limpia.

🛠️ Tecnologías Utilizadas
Motor: Unity Engine (versión 2022.3 LTS).

Lenguaje: C# (Programación orientada a objetos).

UI: TextMeshPro.

Gráficos: Sistema de capas (Sorting Layers) y Render Textures para integración de video.

📋 Estado del Proyecto
✅ Fase 1 (Refactorización): Completada (Código modular y estructura de carpetas).

✅ Fase 2 (Interactividad): Completada (Animaciones, Sensores y Sonido).

✅ Fase 3 (Consolidación UI/UX): En fase de pulido final.

🎓 Nota para Evaluación (Avance 2)
Este proyecto demuestra el cumplimiento de los criterios de la Rúbrica de Avance 2, priorizando la modularidad del código, la estabilidad de las físicas y la fluidez en el flujo de navegación entre menús y escenas.