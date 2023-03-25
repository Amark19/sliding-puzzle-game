using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;

public class filePickerSystem : MonoBehaviour
{
   public void LoadFile(){
        string filetype = "";

        NativeFilePicker.Permission permission = NativeFilePicker.PickFile((path) => {
            Debug.Log("Picked file: " + path);
            if(path != null)
            {
                // Read the bytes of the picked file
                byte[] bytes = File.ReadAllBytes(path);
                // Create a Texture2D from the byte array
                Texture2D texture = new Texture2D(2, 2);
                texture.LoadImage(bytes);
                GameManager.originalImage = texture;
                SceneManager.LoadScene("puzzle_place");
            }
        }, "image/*");
   }

}
