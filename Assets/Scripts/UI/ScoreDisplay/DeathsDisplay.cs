public class DeathsDisplay : IntDisplay
{
	protected override void SubscribeToEvents() {
		this.UpdateText(ScoreManager.Instance.Deaths);
		ScoreManager.Instance.onDeathsUpdate += this.UpdateText;
	}
}
