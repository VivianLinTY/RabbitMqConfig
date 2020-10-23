using System.Text.RegularExpressions;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;

public class RabbitMqConfig : IPreprocessBuildWithReport
{
    private const string UnityGradlePath = "Assets/Plugins/Android/mainTemplate.gradle";
    private const string KeyWord = "**DEPS**";
    public int callbackOrder => 0;  //priority

    public void OnPreprocessBuild(BuildReport report)
    {
        if (report.summary.platform == UnityEditor.BuildTarget.Android)
        {
            InsertImplement();
        }
    }

    void InsertImplement()
    {
        string content = System.IO.File.ReadAllText(UnityGradlePath);

        //There's a disconnection bug in 5.7 & 5.8
        Regex regex = new Regex(@"\s+implementation\s+'com\.rabbitmq:amqp-client:[\w.\[\]]+'");
        if (!regex.IsMatch(content))
        {
            string replacement = $"\timplementation 'com.rabbitmq:amqp-client:4.12.0'\r\n{KeyWord}";
            content = content.Replace(KeyWord, replacement);
        }
        System.IO.File.WriteAllText(UnityGradlePath, content);
    }
}