# Remove Git LFS from a Repository

I've had a hard time trying to remove lfs but thanks to the comments on an issue (*link at bottom*) I managed to remove it. Here follows step-by-step instructions of the approach I used to remove git-lfs:

1. Commit & push everything

2. Create a branch, something like fix/remove-lfs

3. Remove hooks
	- `git lfs uninstall`

4. Delete lfs content from .gitattributes **(do not delete the file)**

5. List all lfs files, `git lfs ls-files`

6. Run `while read line; do git rm --cached "$line"; done < files.txt` to cache contents into a files.txt
	- Make sure you remove the number and asterik on each line, you only want the paths to the files

7. Run `while read line; do git add "$line"; done < files.txt`

8. Run a `git status` and make sure all the files were added properly

9. Commit everything

	```
	git add .gitattributes
	git commit -m "unlfs"
	git push
	```

10. Check if there's no lfs files left with `git lfs ls-files`

11. Remove junk, `rm -rf .git/lfs`

Once the branch (fix/remove-lfs) is merged into develop, just pulling and checking out the new state of the world will work as-expected without git lfs installed. If git lfs is still installed, simply remove the hooks again by: `git lfs uninstall`

## References
> https://github.com/git-lfs/git-lfs/issues/3026