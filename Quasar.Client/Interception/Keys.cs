using System;

namespace Quasar.Client.Interception
{
    public enum Keys : ushort
    {
        // Token: 0x04000328 RID: 808
        Escape = 1,
        // Token: 0x04000329 RID: 809
        One,
        // Token: 0x0400032A RID: 810
        Two,
        // Token: 0x0400032B RID: 811
        Three,
        // Token: 0x0400032C RID: 812
        Four,
        // Token: 0x0400032D RID: 813
        Five,
        // Token: 0x0400032E RID: 814
        Six,
        // Token: 0x0400032F RID: 815
        Seven,
        // Token: 0x04000330 RID: 816
        Eight,
        // Token: 0x04000331 RID: 817
        Nine,
        // Token: 0x04000332 RID: 818
        Zero,
        // Token: 0x04000333 RID: 819
        DashUnderscore,
        // Token: 0x04000334 RID: 820
        PlusEquals,
        // Token: 0x04000335 RID: 821
        Backspace,
        // Token: 0x04000336 RID: 822
        Tab,
        // Token: 0x04000337 RID: 823
        Q,
        // Token: 0x04000338 RID: 824
        W,
        // Token: 0x04000339 RID: 825
        E,
        // Token: 0x0400033A RID: 826
        R,
        // Token: 0x0400033B RID: 827
        T,
        // Token: 0x0400033C RID: 828
        Y,
        // Token: 0x0400033D RID: 829
        U,
        // Token: 0x0400033E RID: 830
        I,
        // Token: 0x0400033F RID: 831
        O,
        // Token: 0x04000340 RID: 832
        P,
        // Token: 0x04000341 RID: 833
        OpenBracketBrace,
        // Token: 0x04000342 RID: 834
        CloseBracketBrace,
        // Token: 0x04000343 RID: 835
        Enter,
        // Token: 0x04000344 RID: 836
        Control,
        // Token: 0x04000345 RID: 837
        A,
        // Token: 0x04000346 RID: 838
        S,
        // Token: 0x04000347 RID: 839
        D,
        // Token: 0x04000348 RID: 840
        F,
        // Token: 0x04000349 RID: 841
        G,
        // Token: 0x0400034A RID: 842
        H,
        // Token: 0x0400034B RID: 843
        J,
        // Token: 0x0400034C RID: 844
        K,
        // Token: 0x0400034D RID: 845
        L,
        // Token: 0x0400034E RID: 846
        SemicolonColon,
        // Token: 0x0400034F RID: 847
        SingleDoubleQuote,
        // Token: 0x04000350 RID: 848
        Tilde,
        // Token: 0x04000351 RID: 849
        LeftShift,
        // Token: 0x04000352 RID: 850
        BackslashPipe,
        // Token: 0x04000353 RID: 851
        Z,
        // Token: 0x04000354 RID: 852
        X,
        // Token: 0x04000355 RID: 853
        C,
        // Token: 0x04000356 RID: 854
        V,
        // Token: 0x04000357 RID: 855
        B,
        // Token: 0x04000358 RID: 856
        N,
        // Token: 0x04000359 RID: 857
        M,
        // Token: 0x0400035A RID: 858
        CommaLeftArrow,
        // Token: 0x0400035B RID: 859
        PeriodRightArrow,
        // Token: 0x0400035C RID: 860
        ForwardSlashQuestionMark,
        // Token: 0x0400035D RID: 861
        RightShift,
        // Token: 0x0400035E RID: 862
        RightAlt = 56,
        // Token: 0x0400035F RID: 863
        Space,
        // Token: 0x04000360 RID: 864
        CapsLock,
        // Token: 0x04000361 RID: 865
        F1,
        // Token: 0x04000362 RID: 866
        F2,
        // Token: 0x04000363 RID: 867
        F3,
        // Token: 0x04000364 RID: 868
        F4,
        // Token: 0x04000365 RID: 869
        F5,
        // Token: 0x04000366 RID: 870
        F6,
        // Token: 0x04000367 RID: 871
        F7,
        // Token: 0x04000368 RID: 872
        F8,
        // Token: 0x04000369 RID: 873
        F9,
        // Token: 0x0400036A RID: 874
        F10,
        // Token: 0x0400036B RID: 875
        F11 = 87,
        // Token: 0x0400036C RID: 876
        F12,
        // Token: 0x0400036D RID: 877
        Up = 72,
        // Token: 0x0400036E RID: 878
        Down = 80,
        // Token: 0x0400036F RID: 879
        Right = 77,
        // Token: 0x04000370 RID: 880
        Left = 75,
        // Token: 0x04000371 RID: 881
        Home = 71,
        // Token: 0x04000372 RID: 882
        End = 79,
        // Token: 0x04000373 RID: 883
        Delete = 83,
        // Token: 0x04000374 RID: 884
        PageUp = 73,
        // Token: 0x04000375 RID: 885
        PageDown = 81,
        // Token: 0x04000376 RID: 886
        Insert,
        // Token: 0x04000377 RID: 887
        PrintScreen = 55,
        // Token: 0x04000378 RID: 888
        NumLock = 69,
        // Token: 0x04000379 RID: 889
        ScrollLock,
        // Token: 0x0400037A RID: 890
        Menu = 93,
        // Token: 0x0400037B RID: 891
        WindowsKey = 91,
        // Token: 0x0400037C RID: 892
        NumpadDivide = 53,
        // Token: 0x0400037D RID: 893
        NumpadAsterisk = 55,
        // Token: 0x0400037E RID: 894
        Numpad7 = 71,
        // Token: 0x0400037F RID: 895
        Numpad8,
        // Token: 0x04000380 RID: 896
        Numpad9,
        // Token: 0x04000381 RID: 897
        Numpad4 = 75,
        // Token: 0x04000382 RID: 898
        Numpad5,
        // Token: 0x04000383 RID: 899
        Numpad6,
        // Token: 0x04000384 RID: 900
        Numpad1 = 79,
        // Token: 0x04000385 RID: 901
        Numpad2,
        // Token: 0x04000386 RID: 902
        Numpad3,
        // Token: 0x04000387 RID: 903
        Numpad0,
        // Token: 0x04000388 RID: 904
        NumpadDelete,
        // Token: 0x04000389 RID: 905
        NumpadEnter = 28,
        // Token: 0x0400038A RID: 906
        NumpadPlus = 78,
        // Token: 0x0400038B RID: 907
        NumpadMinus = 74
    }

}
