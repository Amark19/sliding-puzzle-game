using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
  [SerializeField] private Transform gameTransform;
  [SerializeField] private Transform piecePrefab;

  private List<Transform> pieces;
  private int emptyLocation;
  public int size;
  int puzzleSize_width;
  int puzzleSize_height;
  public static Texture2D originalImage;
  public Texture2D img_scan;
  public bool isImageScan;
  private bool shuffling = false;

    // Create the game setup with size x size pieces.
    private void CreateGamePieces(float gapThickness) {
    // This is the width of each tile.
    float width = 1 / (float)size;
    for (int row = 0; row < size; row++) {
      for (int col = 0; col < size; col++) {
        Transform piece = Instantiate(piecePrefab, gameTransform);
        pieces.Add(piece);
        // Pieces will be in a game board going from -1 to +1.
        piece.localPosition = new Vector3(-1 + (2 * width * col) + width,
                                          +1 - (2 * width * row) - width,
                                          0);
        piece.localScale = new Vector3(((2 * width) - gapThickness),((2 * width) - gapThickness),0.001f);
        piece.name = $"{(row * size) + col}";
        // We want an empty space in the bottom right.
        if ((row == size - 1) && (col == size - 1)) {
          emptyLocation = (size * size) - 1;
          piece.gameObject.SetActive(false);
        } 
        else {
          Texture2D puzzlePiece = new Texture2D(puzzleSize_width,puzzleSize_height);
          Color[] pixels = originalImage.GetPixels(col * puzzleSize_width, ((size-1)*puzzleSize_height) - (row * puzzleSize_height), puzzleSize_width, puzzleSize_height);
          puzzlePiece.SetPixels(pixels);
          puzzlePiece.Apply();
          piece.GetComponent<Renderer>().material.mainTexture = puzzlePiece;
        }
      }
    }
  }

  // Start is called before the first frame update
  void Start() {
    pieces = new List<Transform>();
    if (isImageScan) {
      originalImage = img_scan;
    }
    puzzleSize_width = originalImage.width / (size);
    puzzleSize_height = originalImage.height / (size);
    CreateGamePieces(0.01f);
    Shuffle();
    while (CheckCompletion()){
      Shuffle();
    }
      shuffling = true;
  }

  public void increase_difficulty() {
    shuffling = false;
    Debug.Log("increase difficulty");
    this.GetComponent<UIHandler>().closeNextPanel();
    destroy_pieces();
    size = size + 1;
    puzzleSize_width = originalImage.width / (size);
    puzzleSize_height = originalImage.height / (size);
    pieces = new List<Transform>();
    gameTransform.gameObject.SetActive(true);
    CreateGamePieces(0.01f);
    Shuffle();
    shuffling = true;
  }

  void destroy_pieces() {
    foreach (Transform child in gameTransform) {
      Destroy(child.gameObject);
    }
  }

  void Update() {
    // Check for completion.
    if (CheckCompletion() && shuffling) {
      Debug.Log("completed");
      this.GetComponent<UIHandler>().showNextPanel(size);
    }
    // else{
    //   this.GetComponent<UIHandler>().closeNextPanel();
    // }

    // On click send out ray to see if we click a piece.
    if (Input.GetMouseButtonDown(0)) {
      Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
      RaycastHit hit;
 
    if(Physics.Raycast(ray, out hit)){
        // Go through the list, the index tells us the position.
        for (int i = 0; i < pieces.Count; i++) {
          if (pieces[i].name == hit.transform.gameObject.name) {
            // Check each direction to see if valid move.
            // We break out on success so we don't carry on and swap back again.
            if (SwapIfValid(i, -size, size)) { break; }
            if (SwapIfValid(i, +size, size)) { break; }
            if (SwapIfValid(i, -1, 0)) { break; }
            if (SwapIfValid(i, +1, size - 1)) { break; }
          }
        }
      }
    }
  }

  // colCheck is used to stop horizontal moves wrapping.
  private bool SwapIfValid(int i, int offset, int colCheck) {
    if (((i % size) != colCheck) && ((i + offset) == emptyLocation)) {
      // Swap them in game state.
      (pieces[i], pieces[i + offset]) = (pieces[i + offset], pieces[i]);
      // Swap their transforms.
      (pieces[i].localPosition, pieces[i + offset].localPosition) = ((pieces[i + offset].localPosition, pieces[i].localPosition));
      // Update empty location.
      emptyLocation = i;
      return true;
    }
    return false;
  }

  IEnumerator wait(float duration) {
    yield return new WaitForSeconds(duration);
    this.GetComponent<UIHandler>().showNextPanel(size);
  }

  // We name the pieces in order so we can use this to check completion.
  private bool CheckCompletion() {
    for (int i = 0; i < pieces.Count; i++) {
      if (pieces[i].name != $"{i}") {
        return false;
      }
    }
    return true;
  }

  // Brute force shuffling.
  private void Shuffle() {
    int count = 0;
    int last = 0;
    while (count < (size * size * size)) {
      // Pick a random location.
      int rnd = Random.Range(0, size * size);
      // Only thing we forbid is undoing the last move.
      if (rnd == last) { continue; }
      last = emptyLocation;
      // Try surrounding spaces looking for valid move.
      if (SwapIfValid(rnd, -size, size)) {
        count++;
      } else if (SwapIfValid(rnd, +size, size)) {
        count++;
      } else if (SwapIfValid(rnd, -1, 0)) {
        count++;
      } else if (SwapIfValid(rnd, +1, size - 1)) {
        count++;
      }
    }
    Debug.Log("finish " + CheckCompletion());
  }
}

