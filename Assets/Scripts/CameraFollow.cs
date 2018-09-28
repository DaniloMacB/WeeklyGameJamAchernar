using UnityEngine;

public class CameraFollow : MonoBehaviour {

	public Transform lookAt; // Transform do player

	public float boundX; // Quanto o player poderá se mover no eixo X antes da camera começar a seguir
	public float boundY; // Quanto o player poderá se mover no eixo Y antes da camera começar a seguir
	public float speed; // Velocidade de interpolação da câmera
	private Vector3 desiredPosition; // Vetor com a posição desejada

	void FixedUpdate()
	{
		Vector3 delta = Vector3.zero; // Definindo o delta X

		float dx = lookAt.position.x - transform.position.x; // Delta X é igual a posição do X do jogador - posição x da câmera
		if (dx > boundX || dx < -boundX) // se Delta X for maior que o boundX tanto positivo quanto negativo (para os dois lados)
		{
			if(transform.position.x < lookAt.position.x) // se a posição X da câmera for menor que a posição X do jogador
			{
				delta.x = dx - boundX; // X de delta é igual delta X - boundX
			}
			else
			{
				delta.x = dx + boundX; // senão é igual a delta X + boundX
			}
		}



		float dy = lookAt.position.y - transform.position.y; // Delta Y é igual a posição do Y do jogador - posição y da câmera
		if (dy > boundY || dy < -boundY) // se Delta Y for maior que o boundY tanto positivo quanto negativo (para os dois lados)
		{
			if (transform.position.y < lookAt.position.y) // se a posição Y da câmera for menor que a posição Y do jogador
			{
				delta.y = dy - boundY; // Y de delta é igual delta Y - boundY
			}
			else
			{
				delta.y = dy + boundY; // senão é igual a delta Y + boundY
			}
		}


		desiredPosition = transform.position + delta; // a posição desejada é igual a posição da câmera + delta
		transform.position = Vector3.Lerp(transform.position, desiredPosition, speed); // a posição da câmera é igual a uma interpolação entre a posição da câmera e a posição desejada, adicionando "speed" de velocidade.
	}
}
