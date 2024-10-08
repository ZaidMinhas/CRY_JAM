using System.Collections;
using UnityEngine;
using DG.Tweening;

public class CheatSheet : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] GameObject arrowPrefab;
    private Canvas canvas;
    private AudioSource audioSource;
    private bool firstRun = true;
    public int[] orientation;
    int index = 0;

    [SerializeField] AudioClip poofSound;
    [SerializeField] AudioClip writeSound;

    private void Awake()
    {
        canvas = GetComponentInChildren<Canvas>();
        audioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        generate();
        Disappear();
    }

    
    public void NextAnswer()
    {

        generate();
        StartCoroutine(GenerateAnswer());
    }

    int diff = 0; //dificulty
    int draw_index;

    void generate()
    {
        draw_index = 0;
        index = 0;
        Clear();

        int inputs = Random.Range(4 + (diff * 3) / 4, 6 + diff);
        orientation = new int[inputs];

        for (int i = 0; i < inputs; i++)
        {
            orientation[i] = Random.Range(0, 4);
        }

    }
    IEnumerator GenerateAnswer()
    {
        yield return new WaitForSeconds(0.2f);
        
        while (draw_index < orientation.Length)
        {
            GameObject arrow = Instantiate(arrowPrefab, canvas.transform);
            arrow.transform.localEulerAngles = new Vector3(0, 0, orientation[draw_index] * 90);

            Vector3 originalSize = arrow.transform.localScale;
            arrow.transform.localScale = Vector3.zero;

            arrow.transform.DOScale(originalSize, 0.25f).SetEase(Ease.OutBack);
            audioSource.clip = writeSound;
            audioSource.Play();
            draw_index++;
            yield return new WaitForSeconds(0.3f);
            
        }
        diff += 1;
    }

    void Clear()
    {
        if (orientation == null) { return; }
        foreach (Transform child in canvas.transform)
        {
            Destroy(child.gameObject);
        }
    }

    
    public bool Check(int k)
    {
        if (k == orientation[index])
        {
            index++;
            return true;
        }
        else
        {
            index = 0;
            return false;
        }
    }

    public bool isComplete()
    {
        return (index == orientation.Length);
        
    }

    public bool present = false;
    public void Disappear()
    {
        present = false;
        GetComponent<MeshRenderer>().enabled = false;
        canvas.enabled = false;

        if (firstRun)
        {
            firstRun = false;
            return;
        }
        StopAllCoroutines();

        if (!audioSource.isPlaying)
        {
            
            audioSource.clip = poofSound;
            audioSource.Play();

        }
    }

    public void Reappear()
    {
        present = true;
        GetComponent<MeshRenderer>().enabled = true;
        canvas.enabled = true;

        StartCoroutine(GenerateAnswer());

        if (!audioSource.isPlaying)
        {
            
            audioSource.clip = poofSound;
            audioSource.Play();
        }

    }
}
