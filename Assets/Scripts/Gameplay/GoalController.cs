using UnityEngine;

namespace Gameplay
{
    public class GoalController : MonoBehaviour
    {
        public bool IsPlayer1;
    
        private GameManager gameManager;

        private void Start()
        {
            gameManager = FindObjectOfType<GameManager>();
        }

        //TODO: Research why this is only triggered on the server
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.gameObject.TryGetComponent<BallController>(out var ball))
                return;
        
            gameManager.AddPoint(IsPlayer1);
            ball.Launch();
        }
    }
}
