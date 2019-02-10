using UnityEngine;

public class NoteEdit : MonoBehaviour
{
    public void EditNote()
    {
        NoteManager.instance.EditNote(this.gameObject);
    }
}
