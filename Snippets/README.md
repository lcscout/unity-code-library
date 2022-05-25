## Snippets

Here we have some small pieces of code that don't need a .md file for themselves

- Fix new location - when you rename a repo on github
	- `git remote set-url origin <new link, git@...>`

- Remap one range to another
```
public static float Remap(this float value, float from1, float to1, float from2, float to2) {
	return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
}
```
