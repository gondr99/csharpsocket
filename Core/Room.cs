using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core;

public class Room
{
    //여러 쓰레드에서 접근해도 문제가 없는 딕셔너리 
    public ConcurrentDictionary<string, string> Users { get; } = new ConcurrentDictionary<string, string>();

}
