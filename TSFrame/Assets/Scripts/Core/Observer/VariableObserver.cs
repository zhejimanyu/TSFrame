﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

public sealed partial class Observer : MonoBehaviour
{
    private static Observer _instance = null;
    /// <summary>
    /// 观察者单例
    /// </summary>
    public static Observer Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject obj = new GameObject();
                obj.name = "Observer";
                DontDestroyOnLoad(obj);
                _instance = obj.AddComponent<Observer>();
                obj.hideFlags = HideFlags.HideInHierarchy;
            }
            return _instance;
        }
    }

    /// <summary>
    /// 游戏内所有实体
    /// </summary>
    private Dictionary<int, Entity> _entityDic;
    /// <summary>
    /// 测试工程
    /// </summary>
    private bool _isTest = false;
    /// <summary>
    /// 启动观察者
    /// </summary>
    private bool _isRun = false;

    #region Variable Var

    /// <summary>
    /// 变量游戏物体
    /// </summary>
    private GameObject _variableGameObject;

    #endregion

    #region UI Var

    /// <summary>
    /// ui游戏物体
    /// </summary>
    private GameObject _uiGameObject;
    /// <summary>
    /// ui根目录默认名字
    /// </summary>
    private string _uiRootDefaultName = "";
    /// <summary>
    /// ui根目录
    /// </summary>
    private GameObject _uiCacheRoot;
    /// <summary>
    /// ui根目录集合
    /// key:名称 value:对象
    /// </summary>
    private Dictionary<string, GameObject> _uiRootDic;
    /// <summary>
    /// UI根目录默认常量名称
    /// </summary>
    private const string UI_ROOT_DEFAULT_NAME = "CommonUI";
    #endregion

    #region Game Var

    /// <summary>
    /// 游戏游戏物体
    /// </summary>
    private GameObject _gameGameObject;

    #endregion

    #region Camare Var

    /// <summary>
    /// 相机游戏物体
    /// </summary>
    private GameObject _camareGameObject;
    /// <summary>
    /// 游戏相机
    /// </summary>
    private Camera _camare;

    #endregion

    #region Net Var

    /// <summary>
    /// 网络游戏物体
    /// </summary>
    private GameObject _netGameObject;

    #endregion

    #region Match Var

    /// <summary>
    /// 匹配游戏物体
    /// </summary>
    private GameObject _matchGameObject;
    /// <summary>
    /// 匹配组的字典
    /// </summary>
    private Dictionary<ComponentFlag, Group> _matchGroupDic;

    #endregion

    #region System Var

    /// <summary>
    /// 系统游戏物体
    /// </summary>
    private GameObject _systemGameObject;
    /// <summary>
    /// 初始化执行的系统
    /// </summary>
    private List<ISystem> _systemInitList;
    /// <summary>
    /// 触发执行的系统
    /// </summary>
    private Dictionary<ISystem, List<Entity>> _systemReactiveDic;
    /// <summary>
    /// 循环执行的系统
    /// </summary>
    private List<ISystem> _systemExecuteList;
    #endregion

    #region Entity Var

    /// <summary>
    /// 实体游戏物体
    /// </summary>
    private GameObject _entityGameObject;

    #endregion

    #region Resources Var

    private GameObject _resourcesGameObject;

    private Dictionary<string, ResourcesDto> _resourcesDtoDic;

    /// <summary>
    /// 资源GC的时间(秒)
    /// </summary>
    private int _resourcesTime = 180;

    #endregion

    #region Implement Method

    partial void VariableLoad()
    {
        _entityDic = new Dictionary<int, Entity>();
        _uiRootDic = new Dictionary<string, GameObject>();
        _systemInitList = new List<ISystem>();
        _systemReactiveDic = new Dictionary<ISystem, List<Entity>>();
        _systemExecuteList = new List<ISystem>();
        _matchGroupDic = new Dictionary<ComponentFlag, Group>();
        _resourcesDtoDic = new Dictionary<string, ResourcesDto>();
        _variableGameObject = new GameObject("VariableGameObject");
        _variableGameObject.transform.SetParent(this.transform);
    }

    partial void VariableUpdate()
    {
    }

    #endregion

    #region Class

    class ResourcesDto
    {
        private string _pathName = "";

        public string PathName { get { return _pathName; } }

        private Object _cacheObj = null;

        public Object CacheObj { get { return _cacheObj; } }

        private bool _isAutoRecycle = true;
        /// <summary>
        /// 是否自动回收
        /// </summary>
        public bool IsAutoRecycle { get { return _isAutoRecycle; } set { _isAutoRecycle = value; } }

        private bool _isCanRecycle = false;
        /// <summary>
        /// 是否可以回收
        /// </summary>
        public bool IsCanRecycle { get { return _isCanRecycle; } set { _isCanRecycle = value; } }

        /// <summary>
        /// 最后使用时间
        /// </summary>
        public float LastUseTime { get; set; }

        public ResourcesDto(string path, Object obj, bool isRecycle = true)
        {
            if (string.IsNullOrEmpty(path) || obj == null)
            {
                throw new Exception("Resources cache create failure");
            }
            _isAutoRecycle = isRecycle;
            _pathName = PathName;
            _cacheObj = obj;
        }

        public void Release()
        {
            _cacheObj = null;
            _pathName = null;
        }
        ~ResourcesDto()
        {
            Release();
        }

        public static bool operator ==(ResourcesDto res1, ResourcesDto res2)
        {
            object o1 = res1;
            object o2 = res2;
            if (o1 == null && o2 == null)
            {
                return true;
            }
            if (o1 == null || o2 == null)
            {
                return false;
            }
            return res1.PathName == res2.PathName && res1.CacheObj == res2.CacheObj;
        }
        public static bool operator !=(ResourcesDto res1, ResourcesDto res2)
        {
            return !(res1 == res2);
        }

        public static bool operator ==(ResourcesDto res1, string res2)
        {
            object o1 = res1;
            if (o1 == null && res2 == null)
            {
                return true;
            }
            if (o1 == null || res2 == null)
            {
                return false;
            }
            return res1.PathName == res2;
        }
        public static bool operator !=(ResourcesDto res1, string res2)
        {
            return !(res1 == res2);
        }

        public static bool operator ==(string res2, ResourcesDto res1)
        {
            return res1 == res2;
        }
        public static bool operator !=(string res2, ResourcesDto res1)
        {
            return !(res1 == res2);
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }
        public override int GetHashCode()
        {
            return this.PathName.GetHashCode();
        }


    }

    #endregion
}