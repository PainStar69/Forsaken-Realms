using UnityEngine;

public class ReverseTree : MonoBehaviour
{
    [Header("Bool's")]
    [SerializeField] private bool _rightA, _leftB;
    public bool _right, _left;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            if(_rightA)
            {
                _right = true;
            }
            else if(_leftB)
            {
                _left = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (_rightA)
            {
                _right = false;
            }
            else if (_leftB)
            {
                _left = false;
            }
        }
    }
}
