public class ScoreDisplay : IntDisplay
{
	protected override void SubscribeToEvents() {
		this.UpdateText(ScoreManager.Instance.Score);
		ScoreManager.Instance.onScoreUpdate += this.UpdateText;
	}
}
