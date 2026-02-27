using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.SceneManagement;
using Nexile.JKQuest.SceneManagement;
using HarmonyLib;
using Nexile.JKQuest.UI.Dialogue;

namespace JKQScreenshotsToolMod.Helpers
{
  public class GameHelper
  {
    #region Fields
    // Scene Names
    public static readonly string SplashScreenScene = "SplashScreen";
    public static readonly string MainMenuScene = "MainMenu";
    public static readonly string CoreScene = "CoreScene";

    // References
    private List<GameObject> _coreSceneGameObjects = new List<GameObject>();
    private List<GameObject> _cachedNPCsGameObjects = new List<GameObject>();

    private GameObject _particlePoolingGameObject = null;
    private GameObject _worldParticlesGameObject = null;
    private GameObject _cloudShadowsGameObject = null;

    private Light _mainLight = null;
    private Light _characterLight = null;
    private Light _extraLight = null;

    private static readonly Type JKQSceneManagerTypeRef = typeof(Nexile.JKQuest.SceneManagement.SceneManager);
    private static readonly FieldInfo JKQSceneManagerLoadedScenesFieldInfo = AccessTools.Field(JKQSceneManagerTypeRef, "loadedScenes");
    #endregion


    #region Initialization Methods
    public void Init()
    {
      UnityEngine.SceneManagement.SceneManager.GetSceneByName(CoreScene).GetRootGameObjects(_coreSceneGameObjects);

      foreach (GameObject go in _coreSceneGameObjects)
      {
        if (go.name == "LateLoad")
        {
          // Apparently, the Pooling gameobject gets instantiated late. Moved this to the AreParticlesDisabled field for lazy loading
          //_worldParticlesGameObject = go.transform.Find("Weather Effects").gameObject;
          //_cloudShadowsGameObject = go.transform.Find("Cloud Shadows").gameObject;

          _mainLight = go.transform.Find("AreaSettings/Lights/Light_Main").gameObject.GetComponent<Light>();
          _characterLight = go.transform.Find("AreaSettings/Lights/Light_Character").gameObject.GetComponent<Light>();
          _extraLight = go.transform.Find("AreaSettings/Lights/Light_Extra1").gameObject.GetComponent<Light>();

          break;
        }

        //if (go.name == "Pooling")
        //{
        //  _particlePoolingGameObject = go;
        //  continue;
        //}
      }
    }

    private void GetParticleGameObjects()
    {
      if (_particlePoolingGameObject == null || _worldParticlesGameObject == null || _cloudShadowsGameObject == null)
      {
        UnityEngine.SceneManagement.SceneManager.GetSceneByName(CoreScene).GetRootGameObjects(_coreSceneGameObjects);

        foreach (GameObject go in _coreSceneGameObjects)
        {
          if (go.name == "LateLoad")
          {
            _worldParticlesGameObject = go.transform.Find("Weather Effects").gameObject;
            _cloudShadowsGameObject = go.transform.Find("Cloud Shadows").gameObject;

            continue;
          }

          if (go.name == "Pooling")
          {
            _particlePoolingGameObject = go;
            continue;
          }
        }
      }
    }

    public void Clear()
    {
      _coreSceneGameObjects.Clear();

      _particlePoolingGameObject = null;
      _worldParticlesGameObject = null;
      _cloudShadowsGameObject = null;

      _mainLight = null;
      _characterLight = null;
      _extraLight = null;
    }
    #endregion


    #region Hide Methods
    public void HidePlayers(bool hide)
    {
      _coreSceneGameObjects.Clear();
      UnityEngine.SceneManagement.SceneManager.GetSceneByName(CoreScene).GetRootGameObjects(_coreSceneGameObjects);

      foreach (GameObject go in _coreSceneGameObjects)
      {
        if (!go.name.StartsWith("JumpKingGlue")) continue;

        GameObject animatorParrot = go.transform.Find("centerOfMass/LookScaler/AnimatorParrot").gameObject;
        if (animatorParrot != null)
        {
          animatorParrot.SetActive(!hide);
        }

        GameObject playerLight = go.transform.Find("player point light").gameObject;
        if (playerLight != null)
        {
          playerLight.SetActive(!hide);
        }
      }
    }
    public void HideEnemies(bool hide)
    {
      _coreSceneGameObjects.Clear();
      UnityEngine.SceneManagement.SceneManager.GetSceneByName(CoreScene).GetRootGameObjects(_coreSceneGameObjects);

      foreach (GameObject go in _coreSceneGameObjects)
      {
        if (!go.name.Contains("Glue")) continue;
        if (go.name.StartsWith("JumpKingGlue")) continue;

        Transform[] componentsInChildren = go.GetComponentsInChildren<Transform>(true);
        for (int i = 0; i < componentsInChildren.Length; i++)
        {
          if (componentsInChildren[i].gameObject.name.Contains("Parrot"))
          {
            componentsInChildren[i].gameObject.SetActive(!hide);
          }
        }
      }
    }
    public void HideNPCs(bool hide)
    {
      if (!hide)
      {
        foreach (GameObject hiddenNPC in _cachedNPCsGameObjects)
        {
          if (hiddenNPC == null) continue;
          hiddenNPC.gameObject.SetActive(true);
        }

        _cachedNPCsGameObjects.Clear();
        return;
      }


      var loadedScenesInfo = (Dictionary<string, LoadedSceneInfo>)JKQSceneManagerLoadedScenesFieldInfo.GetValue(Nexile.JKQuest.SceneManagement.SceneManager.Instance);

      foreach (LoadedSceneInfo loadedSceneInfo in loadedScenesInfo.Values)
      {
        string sceneName = string.Empty;
        int index = loadedSceneInfo.Path.LastIndexOf('/');

        sceneName = (index >= 0) ? loadedSceneInfo.Path.Substring(index + 1) : loadedSceneInfo.Path;
        if (sceneName.EndsWith(".unity")) sceneName = sceneName.Substring(0, sceneName.Length - 6);

        Scene loadedScene = UnityEngine.SceneManagement.SceneManager.GetSceneByName(sceneName);
        if (!loadedScene.IsValid()) continue;


        List<GameObject> sceneGO = new List<GameObject>();
        loadedScene.GetRootGameObjects(sceneGO);

        foreach (GameObject go in sceneGO)
        {
          NPCDialogue[] npcsFound = go.GetComponentsInChildren<NPCDialogue>(false);

          foreach (NPCDialogue npc in npcsFound)
          {
            _cachedNPCsGameObjects.Add(npc.gameObject);
            npc.gameObject.SetActive(false);
          }
        }
      }
    }
    #endregion

    #region Disable Methods
    public bool AreParticlesDisabled
    {
      get
      {
        GetParticleGameObjects();

        if (_particlePoolingGameObject == null || _worldParticlesGameObject == null || _cloudShadowsGameObject == null) return false;
        return !_particlePoolingGameObject.activeSelf;
      }
      set
      {
        GetParticleGameObjects();

        if (_particlePoolingGameObject == null || _worldParticlesGameObject == null || _cloudShadowsGameObject == null) return;

        _particlePoolingGameObject.SetActive(!value);
        _worldParticlesGameObject.SetActive(!value);
        _cloudShadowsGameObject.SetActive(!value);
      }
    }
    #endregion

    #region Lighting
    public Quaternion MainLightingRotation
    {
      get => _mainLight.transform.rotation;
      set => _mainLight.transform.rotation = value;
    }
    public Color MainLightingColor
    {
      get => _mainLight.color;
      set => _mainLight.color = value;
    }
    public float MainLightingIntensity
    {
      get => _mainLight.intensity;
      set => _mainLight.intensity = value;
    }

    public Color CharacterLightingColor
    {
      get => _characterLight.color;
      set => _characterLight.color = value;
    }
    public float CharacterLightingIntensity
    {
      get => _characterLight.intensity;
      set => _characterLight.intensity = value;
    }

    public Color ExtraLightingColor
    {
      get => _extraLight.color;
      set => _extraLight.color = value;
    }
    public float ExtraLightingIntensity
    {
      get => _extraLight.intensity;
      set => _extraLight.intensity = value;
    }
    #endregion
  }
}