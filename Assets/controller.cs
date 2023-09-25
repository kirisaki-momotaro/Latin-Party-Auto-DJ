using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

using System.IO;
using System;
using System.Text;
using SFB;





public class controller : MonoBehaviour
{
    // Start is called before the first frame update

    public TMP_InputField salsaNum;
    public TMP_InputField bachataNum;
    public TMP_InputField kizombaNum;
    
    public TextMeshProUGUI infoText;
    public int snum;
    public int bnum;
    public int knum;
    public string salsaDir;
    public string bachataDir;
    public string kizombaDir;
    public AudioSource audioSource;

    public string[] salsaSongs;
    public string[] bachataSongs;
    public string[] kizombaSongs;
    public WWW www;

    public int[] songChange;
    public int songIndex;
    public bool isPaused;
    public bool isStopped;


    public List<int> salsaExclusionList = new List<int>();
    public List<int> bachataExclusionList = new List<int>();
    public List<int> kizombaExclusionList = new List<int>();




    void Start()
    {
        
        salsaDir = null; bachataDir = null; kizombaDir = null;
        snum = 0;bnum = 0;knum = 0;
        songIndex= 0;
        isPaused = false;
        isStopped = true;




    }

    IEnumerator LoadAudioClip(string path)
    {
        using (WWW www = new WWW(path))
        {
            yield return www;
            if (www.error == null)
            {
                audioSource.clip = www.GetAudioClip();
                audioSource.Play();
            }
            else
            {
                Debug.LogError("Error loading audio: " + www.error);
            }
        }
    }

    public void sDirClicked()
    {
        salsaDir= StandaloneFileBrowser.OpenFolderPanel("Select Folder", Application.dataPath, false)[0];
        //salsaDir = EditorUtility.OpenFolderPanel("Select Salsa Directory", "", "");
        salsaSongs= Directory.GetFiles(salsaDir);
        Debug.Log(salsaDir);
        Debug.Log(salsaSongs[4]);
        /*for (int i = 0; i < salsaSongs.Length; i++)
        {
            var aStringBuilder = new StringBuilder(salsaSongs[i]);
            aStringBuilder.Remove(salsaDir.Length,1);
            aStringBuilder.Insert(salsaDir.Length,"/");
            salsaSongs[i]= aStringBuilder.ToString();
        }
        Debug.Log(salsaSongs[4]);*/
    }
    public void bDirClicked()
    {
        bachataDir = StandaloneFileBrowser.OpenFolderPanel("Select Folder", Application.dataPath, false)[0];
        Debug.Log(bachataDir);
        bachataSongs = Directory.GetFiles(bachataDir);
        Debug.Log(bachataDir);
        /*for (int i = 0; i < bachataSongs.Length; i++)
        {
            var aStringBuilder = new StringBuilder(bachataSongs[i]);
            aStringBuilder.Remove(bachataDir.Length, 1);
            aStringBuilder.Insert(bachataDir.Length, "/");
            bachataSongs[i] = aStringBuilder.ToString();
        }
        Debug.Log(bachataSongs[4]);*/
    }
    public void kDirClicked()
    {
        kizombaDir = StandaloneFileBrowser.OpenFolderPanel("Select Folder", Application.dataPath, false)[0];
        Debug.Log(kizombaDir);
        kizombaSongs = Directory.GetFiles(kizombaDir);
        Debug.Log(kizombaDir);
        /*for (int i = 0; i < kizombaSongs.Length; i++)
        {
            var aStringBuilder = new StringBuilder(kizombaSongs[i]);
            aStringBuilder.Remove(kizombaDir.Length, 1);
            aStringBuilder.Insert(kizombaDir.Length, "/");
            kizombaSongs[i] = aStringBuilder.ToString();
        }
        Debug.Log(kizombaSongs[4]);*/
    }
    public int nextSongIndex(int i)
    {
        int randomNumber;
        if (i == 1)
        {            
            do
            {
                
                randomNumber = UnityEngine.Random.Range(0, salsaSongs.Length); 
            } while (salsaExclusionList.Contains(randomNumber)); 
        }else if(i == 2)
        {
            do
            {

                randomNumber = UnityEngine.Random.Range(0, bachataSongs.Length);
            } while (bachataExclusionList.Contains(randomNumber));
        }
        else
        {
            do
            {

                randomNumber = UnityEngine.Random.Range(0, kizombaSongs.Length);
            } while (kizombaExclusionList.Contains(randomNumber));
        }
        

        return randomNumber;
    }
    public void nextButtonClicked()
    {

        isPaused = false;
        isStopped = false;
        if (songIndex>songChange.Length-1)
        {
            songIndex = 0;
        }
        string song;
        if (songChange[songIndex] == 1)
        {
            int randomIndex = nextSongIndex(1);
            song = salsaSongs[randomIndex];
            salsaExclusionList.Add(randomIndex);
        }
        else if (songChange[songIndex] == 2)
        {
            int randomIndex = nextSongIndex(2);
            song = bachataSongs[randomIndex];
            bachataExclusionList.Add(randomIndex);
        }
        else
        {
            int randomIndex = nextSongIndex(3);
            song = kizombaSongs[randomIndex];
            kizombaExclusionList.Add(randomIndex);
        }
        AudioClip.Destroy(audioSource.clip);
        StartCoroutine(LoadAudioClip("file:///" + song));
        songIndex += 1;




    }
    public void pauseButtonClicked()
    {


        if (audioSource.isPlaying)
        {
            isPaused = true;
            infoText.text = "Paused";
            audioSource.Pause();
        }
        else
        {
            isPaused = false;
            infoText.text = "Playing";
            audioSource.UnPause();
        }
    }
    public void stopButtonClicked()
    {
        infoText.text = "Stopped";
        isStopped = true;
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }
    static int[] GenerateArray(int x, int y, int z)
    {
        int[] array = new int[x + y + z];

        // Fill the array with the specified numbers and counts
        int currentIndex = 0;
        for (int i = 0; i < x; i++)
        {
            array[currentIndex] = 1;
            currentIndex++;
        }
        for (int i = 0; i < y; i++)
        {
            array[currentIndex] = 2;
            currentIndex++;
        }
        for (int i = 0; i < z; i++)
        {
            array[currentIndex] = 3;
            currentIndex++;
        }

        return array;
    }

public void startButtonClicked()
    {
        if (salsaDir == null || bachataDir == null || kizombaDir == null)
        {
            infoText.text = "Please enter directories";


        }
        else if (snum == 0 && bnum == 0 && knum == 0)
        {
            infoText.text = "Please enter dance repeats";
        }
        else
        {
            isPaused = false;
            isStopped = false;
            infoText.text = "Playing";
            songChange=GenerateArray(snum, bnum, knum);
            string song;
            if (songChange[0]==1)
            {
                int randomIndex = UnityEngine.Random.Range(0, salsaSongs.Length);
                song = salsaSongs[randomIndex];
            }else if (songChange[0]==2) {
                int randomIndex = UnityEngine.Random.Range(0, bachataSongs.Length);
                song = bachataSongs[randomIndex];
            }
            else
            {
                int randomIndex = UnityEngine.Random.Range(0, kizombaSongs.Length);
                song = kizombaSongs[randomIndex];
            }

            StartCoroutine(LoadAudioClip("file:///" + song));
            if (songIndex > songChange.Length-1)
            {
                songIndex = 0;
            }
            else
            {
                songIndex += 1;
            }
            
        }
        
    }

    // Update is called once per frame
    void Update()
    {

        snum = int.Parse(salsaNum.text);
        bnum = int.Parse(bachataNum.text);
        knum = int.Parse(kizombaNum.text);
        // Debug.Log(snum);
        if(bachataExclusionList.Count >= bachataSongs.Length-1)
        {
            bachataExclusionList.Clear();
        }
        if (salsaExclusionList.Count >= salsaSongs.Length-1)
        {
            salsaExclusionList.Clear();
        }
        if (kizombaExclusionList.Count >= kizombaSongs.Length-1)
        {
            kizombaExclusionList.Clear();
        }
        if (audioSource != null && !audioSource.isPlaying)
        {
            if(isStopped==false && isPaused==false)
            {
                
                if (songIndex > songChange.Length-1)
                {
                    songIndex = 0;
                }
                string song;
                if (songChange[songIndex] == 1)
                {
                    int randomIndex = nextSongIndex(1);
                    song = salsaSongs[randomIndex];
                    salsaExclusionList.Add(randomIndex);
                }
                else if (songChange[songIndex] == 2)
                {
                    int randomIndex = nextSongIndex(2);
                    song = bachataSongs[randomIndex];
                    bachataExclusionList.Add(randomIndex);
                }
                else
                {
                    int randomIndex = nextSongIndex(3);
                    song = kizombaSongs[randomIndex];
                    kizombaExclusionList.Add(randomIndex);
                }
                AudioClip.Destroy(audioSource.clip);
                StartCoroutine(LoadAudioClip("file:///" + song));
                songIndex += 1;




            }
            
        }


    }
}
