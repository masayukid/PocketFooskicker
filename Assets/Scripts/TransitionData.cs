using System;
using System.Collections.Generic;
using System.Linq;

public class TransitionData
{
    private readonly IReadOnlyDictionary<string, object> _dataDictionary;

    public TransitionData(params (string key, object value)[] dataDictionary)
    {
        if (dataDictionary == null)
        {
            throw new ArgumentNullException("引数 dataDictionary が null です。キーと値のペアを指定してください。");
        }

        _dataDictionary = dataDictionary.ToDictionary(item => item.key, item => item.value);
    }

    public T GetValueOrDefault<T>(string key, T defaultValue)
    {
        if (_dataDictionary.TryGetValue(key, out var value))
        {
            if (value is T typedValue)
            {
                return typedValue;
            }

            throw new ArgumentException($"キー '{key}' に関連付けられた値は、期待される型 {typeof(T).FullName} ではありません。");
        }

        return defaultValue;
    }
}