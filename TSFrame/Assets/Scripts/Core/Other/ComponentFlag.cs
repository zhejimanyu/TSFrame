﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// 组件的标记为
/// </summary>
public struct ComponentFlag
{
    private Int64 _systemLowFlag;
    private Int64 _systemHighFlag;
    private Int64 _playerlowFlag;
    private Int64 _playerhighFlag;

    public Int64 PlayerLowFlag { get { return _playerlowFlag; } private set { _playerlowFlag = value; } }

    public Int64 PlayerHighFlag { get { return _playerhighFlag; } private set { _playerhighFlag = value; } }

    public Int64 SystemLowFlag { get { return _systemLowFlag; } private set { _systemLowFlag = value; } }

    public Int64 SystemHighFlag { get { return _systemHighFlag; } private set { _systemHighFlag = value; } }
    public bool HasFlag(ComponentFlag flag)
    {
        return (this.SystemLowFlag & flag.SystemLowFlag) == flag.SystemLowFlag && (this.SystemHighFlag & flag.SystemHighFlag) == flag.SystemHighFlag && (this.PlayerLowFlag & flag.PlayerLowFlag) == flag.PlayerLowFlag && (this.PlayerHighFlag & flag.PlayerHighFlag) == flag.PlayerHighFlag;
    }
    public Boolean HasFlag(Int64 flag)
    {
        if ((flag & ComponentIds.SYSTEM_LOW_FLAG) == ComponentIds.SYSTEM_LOW_FLAG)
        {
            //系统低位
            return (flag & SystemLowFlag) == flag;
        }
        else if ((flag & ComponentIds.SYSTEM_HIGH_FLAG) == ComponentIds.SYSTEM_HIGH_FLAG)
        {
            //系统高位
            return (flag & SystemHighFlag) == flag;
        }
        else if ((flag & ComponentIds.PLAYER_LOW_FLAG) == ComponentIds.PLAYER_LOW_FLAG)
        {
            //用户低位
            return (flag & PlayerLowFlag) == flag;
        }
        else
        {
            //用户高位
            return (flag & PlayerHighFlag) == flag;
        }
    }

    public ComponentFlag SetFlag(Int64 flag)
    {
        if ((flag & ComponentIds.SYSTEM_LOW_FLAG) == ComponentIds.SYSTEM_LOW_FLAG)
        {
            //系统低位
            SystemLowFlag = flag | SystemLowFlag;
        }
        else if ((flag & ComponentIds.SYSTEM_HIGH_FLAG) == ComponentIds.SYSTEM_HIGH_FLAG)
        {
            //系统高位
            SystemHighFlag = flag | SystemHighFlag;
        }
        else if ((flag & ComponentIds.PLAYER_LOW_FLAG) == ComponentIds.PLAYER_LOW_FLAG)
        {
            //用户低位
            PlayerLowFlag = flag | PlayerLowFlag;
        }
        else
        {
            //用户高位
            PlayerHighFlag = flag | PlayerHighFlag;
        }
        return this;
    }
    public ComponentFlag RemoveFlag(Int64 flag)
    {
        if ((flag & ComponentIds.SYSTEM_LOW_FLAG) == ComponentIds.SYSTEM_LOW_FLAG)
        {
            //系统低位
            SystemLowFlag = flag ^ SystemLowFlag;
        }
        else if ((flag & ComponentIds.SYSTEM_HIGH_FLAG) == ComponentIds.SYSTEM_HIGH_FLAG)
        {
            //系统高位
            SystemHighFlag = flag ^ SystemHighFlag;
        }
        else if ((flag & ComponentIds.PLAYER_LOW_FLAG) == ComponentIds.PLAYER_LOW_FLAG)
        {
            //用户低位
            PlayerLowFlag = flag ^ PlayerLowFlag;
        }
        else
        {
            //用户高位
            PlayerHighFlag = flag ^ PlayerHighFlag;
        }
        return this;
    }
    public static bool operator ==(ComponentFlag cf1, ComponentFlag cf2)
    {
        return cf1.SystemLowFlag == cf2.SystemLowFlag && cf1.SystemHighFlag == cf2.SystemHighFlag && cf1.PlayerLowFlag == cf2.PlayerLowFlag && cf1.PlayerHighFlag == cf2.PlayerHighFlag;
    }
    public static ComponentFlag operator &(ComponentFlag cf1, ComponentFlag cf2)
    {
        ComponentFlag temp = new ComponentFlag();
        temp.SystemLowFlag = cf1.SystemLowFlag & cf2.SystemLowFlag;
        temp.SystemHighFlag = cf1.SystemHighFlag & cf2.SystemHighFlag;
        temp.PlayerLowFlag = cf1.PlayerLowFlag & cf2.PlayerLowFlag;
        temp.PlayerHighFlag = cf1.PlayerHighFlag & cf2.PlayerHighFlag;
        return temp;
    }



    public static bool operator !=(ComponentFlag cf1, ComponentFlag cf2)
    {
        return !(cf1 == cf2);
    }
    public override bool Equals(object obj)
    {
        return base.Equals(obj);
    }
    public override int GetHashCode()
    {
        return SystemLowFlag.GetHashCode() ^ SystemHighFlag.GetHashCode() ^ PlayerLowFlag.GetHashCode() ^ PlayerHighFlag.GetHashCode();
    }
}