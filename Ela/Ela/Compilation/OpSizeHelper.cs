using System;

namespace Ela.Compilation
{
    internal static class OpSizeHelper
    {
        internal static int[] OpSize = 
		{
			1, //Nop
			1, //Len
			1, //Pushunit
			1, //Pushelem
            1, //Pushfld
            1, //Hasfld
			1, //PushI4_0
			1, //PushI1_0
			1, //PushI1_1
			1, //Pop
			1, //Pushstr_0
			1, //Genfin
			1, //Cons
			1, //Tail
			1, //Head
			1, //Ret
			1, //Concat
			1, //Add
			1, //Mul
			1, //Div
            1, //Quot
			1, //Rem
            1, //Mod
			1, //Pow
			1, //Sub
			1, //Shr
			1, //Shl
			1, //Ceq
			1, //Cneq
			1, //Clt
			1, //Cgt
			1, //Cgteq
			1, //Clteq
			1, //AndBw
			1, //OrBw
			1, //Xor
			1, //Not
			1, //Neg
			1, //NotBw
			1, //Dup
			1, //Swap
			1, //Newlazy
			1, //Newlist
			1, //Newtup_2
			1, //Stop
			1, //NewI8
			1, //NewR8
			1, //Leave
			1, //Flip,
			1, //LazyCall
			1, //Call
			1, //Callt
            1, //Throw
			1, //Rethrow
			1, //Force
			1, //Isnil
			1, //Show
            1, //Addmbr            
            1, //Traitch
            1, //Skiptag
			1, //Newtype
            1, //Newtype0
            1, //Newtype1
            1, //Newtype2
            1, //Ctype            
            1, //Disp        
            1, //Ctx

            5, //Newconst
            5, //Api
            5, //Api2
            5, //Untag
			5, //Reccons
			5, //Tupcons
            5, //Ctorid
            5, //Typeid
            5, //Classid
            5, //Newfunc
            5, //Newmod
			5, //Pushext
            5, //Newrec
			5, //Newtup
			5, //Failwith
			5, //Start
			5, //Pushstr
			5, //PushCh
			5, //PushI4
			5, //PushR4
            5, //Pushloc
			5, //Pushvar
			5, //Poploc
			5, //Popvar
			5, //Runmod
			5, //Br
			5, //Brtrue
			5, //Brfalse
			5, //Newfun			
		};
    }
}