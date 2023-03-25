using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIHandler : MonoBehaviour
{
    public GameObject errorCard;
    public GameObject refImgPanel;
    public GameObject completePanel;
    public GameObject refBtn;
    public Text puzzle_size;

    public void showError()
    {
        errorCard.SetActive(true);
    }

    public void showRefImage()
    {
        refImgPanel.SetActive(true);
        refImgPanel.transform.GetChild(0).gameObject.GetComponent<RawImage>().texture = GameManager.originalImage;
    }

    public void closeError()
    {
        errorCard.SetActive(false);
        SceneManager.LoadScene("home");
    }

    public void goToImgScan()
    {
        SceneManager.LoadScene("puzzle_imgScan");
    }
    public void goTobuiltInImagePuzzle()
    {
        SceneManager.LoadScene("puzzle_inbuilt");
    }

    public void closeRefImgPanel()
    {
        refImgPanel.SetActive(false);
    }

    public void showNextPanel(int size)
    {
        Debug.Log(size);
        puzzle_size.text = $"{size}x{size} puzzle";
        completePanel.SetActive(true);
        refBtn.SetActive(false);
    }

    public void closeNextPanel()
    {
        completePanel.SetActive(false);
        refBtn.SetActive(true);
    }

    void Update()
    {

        if (Input.GetKey(KeyCode.Escape))
        {
            if (SceneManager.GetActiveScene().name == "home")
            {
                Application.Quit();
            }
            else
            {
            SceneManager.LoadScene("home");
            return;

            }
        }
    }
 
}
