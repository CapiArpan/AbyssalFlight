# 🐉 Abyssal Flight - Prototipo de Videojuego 2D
**Desarrollado para:** Programación de Videojuegos | Universidad Santo Tomás.

## 📝 Descripción
Un arcade de scroll vertical con estética **Grimdark**. El jugador controla un dragón en un descenso infinito por un abismo gótico, gestionando energía y esquivando obstáculos para sobrevivir.

## 🚀 Características Técnicas (Rúbrica de Evaluación)
Este proyecto ha sido desarrollado bajo estándares de arquitectura limpia y optimización:

* [cite_start]**Arquitectura C#:** Implementación de scripts modulares y uso de `[SerializeField]` para una configuración eficiente desde el Inspector.
* [cite_start]**Object Pooling:** Sistema de gestión de memoria para obstáculos y comida, evitando el uso de `Instantiate/Destroy` para optimizar el Garbage Collector.
* [cite_start]**Físicas Avanzadas:** Uso diferenciado de `Rigidbody2D` (Kinematic para el jugador) y `OnTriggerEnter2D` para una detección de colisiones fluida.
* **Efecto Parallax:** Sistema de múltiples capas de fondo con velocidades variables para generar profundidad visual.
* [cite_start]**UI Dinámica:** Canvas configurado con anclajes (Anchors) para soporte de múltiples resoluciones y navegación entre escenas (Menú -> Juego).

## 🎮 Controles
* **A / D o Flechas:** Movimiento Horizontal.
* **R:** Reiniciar el juego tras perder.

## 🛠️ Tecnologías
* **Motor:** Unity 2022.3+ (o tu versión actual).
* **Lenguaje:** C# con estándares de programación orientada a objetos.
* **Arte:** Pixel Art inspirado en la estética gótica medieval.
