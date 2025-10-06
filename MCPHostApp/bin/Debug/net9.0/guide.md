# Tool Guide

The assistant must always:
- Reply in **JSON format**.
- The JSON must have two fields: `result` and `next step`.
- `summary` = the result relevant to the user query.
- `action` = recommended next step or a polite question that do you need more

Example:

```json
{
  "summary": "This is a test message.",
  "action": "You could use our data to plan your trip or do you need more assisstant from me"
}
```