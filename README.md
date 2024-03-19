# Assumptions and README

### Logger - Singleton:
- Only using one level of logging due to a simple program. In a large-scale production, we would have many more levels of logs.

### DocumentReader Class:
- `CountHowManyTimesFileNameAppearsInsideFile` is using `ReadToEnd()`, which could be improved by reading line by line, mainly for large files.
- `CloseFile()` is setting `streamReader = null` and `fileStream = null` when closing streams, which could be optimized to avoid producing unnecessary garbage.
- `maxAttempts` and `attempts` are trying to prevent abuse of the app. This is a very simple implementation that should be expanded upon using other validation techniques like image and text-based CAPTCHA or reCAPTCHA.

### FakeDocumentReader Class:
- A very sloppy and fast way to make the tests easier to control. Mainly adding breaks in the while loop prompting the user for input.

### Overall
- The `Messages()` class might seem overkill since it's barely used, but it's there to demonstrate the concept of "Separation of Concerns".
- I hope we can discuss the code at a late phase, I'm aware of a few thing that can definitely be improved. E.g. not having and loops inside the private methods and instead make them return a value. Then put all logic in one method instead. Due to lack of time I just need to send this in as is. Hope it's fine! :) Cheers! 



