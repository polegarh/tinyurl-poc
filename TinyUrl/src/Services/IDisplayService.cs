using TinyUrl.DTOs;

namespace TinyUrl.Services;

public interface IDisplayService
{
    public void Start();
    public void GetOptions();
    public Option GetUserOption();
    public void EvaluateDecision(Option option);
}