#!/bin/bash
echo "Running deployment script..."

CURRENT_COMMIT=`git rev-parse HEAD`

echo "Cloning master branch..."

git clone -b master "https://${GH_TOKEN}@${GH_REF}" _deploy > /dev/null 2>&1 || exit 1

echo "Clean deploy dir"
rm _deploy/* -rf

echo "Copying built files"
cp -R output/. _deploy
cd _deploy

echo "Committing and pushing to GH"

git status
git config user.name "Travis-CI"
git config user.email "travis@kamranicus.com"
git add -A
git commit --allow-empty -m "Deploying site for $CURRENT_COMMIT" || exit 1
git push origin master > /dev/null 2>&1 || exit 1

echo "Pushed deployment successfully"
exit 0