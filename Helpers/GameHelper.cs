using AraSamples;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    List<GameObject> coreSceneGameObjects = new List<GameObject>();

    private GameObject _particlePoolingGameObject = null;
    private GameObject _worldParticlesGameObject = null;
    private GameObject _cloudShadowsGameObject = null;

    private Light _mainLight = null;
    private Light _characterLight = null;
    private Light _extraLight = null;
    #endregion


    #region Initialization Methods
    public void Init()
    {
      SceneManager.GetSceneByName(CoreScene).GetRootGameObjects(coreSceneGameObjects);

      foreach (GameObject go in coreSceneGameObjects)
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

    public void Clear()
    {
      coreSceneGameObjects.Clear();

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
      coreSceneGameObjects.Clear();
      SceneManager.GetSceneByName(CoreScene).GetRootGameObjects(coreSceneGameObjects);

      foreach (GameObject go in coreSceneGameObjects)
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
      coreSceneGameObjects.Clear();
      SceneManager.GetSceneByName(CoreScene).GetRootGameObjects(coreSceneGameObjects);

      foreach (GameObject go in coreSceneGameObjects)
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
    #endregion

    #region Disable Methods
    public bool AreParticlesDisabled
    {
      get
      {
        if (_particlePoolingGameObject == null || _worldParticlesGameObject == null || _cloudShadowsGameObject == null)
        {
          SceneManager.GetSceneByName(CoreScene).GetRootGameObjects(coreSceneGameObjects);

          foreach (GameObject go in coreSceneGameObjects)
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

        if (_particlePoolingGameObject == null || _worldParticlesGameObject == null || _cloudShadowsGameObject == null) return false;

        return !_particlePoolingGameObject.activeSelf;
      }
      set
      {
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