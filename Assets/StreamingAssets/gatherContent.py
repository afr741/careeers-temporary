# Gather content key - 73eb265e-d574-497f-a798-c248d37e1186
import requests
import json
import codecs

projectId = 312999
baseUrl = "https://api.gathercontent.com/"
headers = {
    "Accept": "application/vnd.gathercontent.v2+json",
    "Authorization": "Basic YXJha2htYXRvdkBuZ3hpbnRlcmFjdGl2ZS5jb206MmI3Mjk5NjQtM2U3MC00MGZmLTg4ZjUtMzI0YWM5YzEwNzRk"
}


def removeHtmlFormatting(content_text):
    while(True):
        startIndex = content_text.find("<")
        endIndex = content_text.find(">")
        if(startIndex == -1 or endIndex == -1):
            break
        preString = ""
        postString = ""
        if(startIndex >= 1):
            preString = content_text[0:startIndex]
        if(endIndex <= len(content_text)-2):
            postString = content_text[endIndex+1:]
        content_text = preString + postString
    return content_text


def removeDoubleSpaces(content_text):
    while(True):
        doubleSpaceIndex = content_text.find("  ")
        if(doubleSpaceIndex == -1):
            break
        content_text = content_text.replace("  ", " ")
    return content_text


def cleanupText(content_text):
    if(not content_text or content_text == []):
        return content_text
    return removeDoubleSpaces(removeHtmlFormatting(content_text))


def getItems():
    url = baseUrl + "projects/" + str(projectId) + "/items"
    querystring = {"include": "folder_name"}
    response = requests.request(
        "GET", url, headers=headers, params=querystring)
    response_json = json.loads(response.text)
    return response_json


def getFolders():
    url = baseUrl + "projects/" + str(projectId) + "/folders"
    response = requests.request(
        "GET", url, headers=headers)
    response_json = json.loads(response.text)
    return response_json


def getItemDetailsRaw(itemId):
    print("Getting item " + str(itemId))
    querystring = {"include": "structure"}
    url = baseUrl + "items/" + str(itemId)
    response = requests.request(
        "GET", url, headers=headers, params=querystring)
    return json.loads(response.text)


def getItemDetails(itemId, folderName):
    # id[ name, languages[ content1[ label, text]]]
    itemDetailsRaw = getItemDetailsRaw(itemId)
    content = {}
    content["id"] = itemId
    content["name"] = itemDetailsRaw["data"]["name"]
    content["foldername"] = folderName
    # printJson(itemDetailsRaw["data"])

    content["text"] = []
    for group in itemDetailsRaw["data"]["structure"]["groups"]:
        for field in group["fields"]:
            text = {}
            text["label"] = field["label"]
            uuid = field["uuid"]
            # printJson(isinstance(itemDetailsRaw["data"]["content"][uuid],str))
            if(not uuid in itemDetailsRaw["data"]["content"]):
                continue
            if(not isinstance(itemDetailsRaw["data"]["content"][uuid], str)):
                continue
            text["content"] = cleanupText(
                itemDetailsRaw["data"]["content"][uuid])
            content["text"].append(text)
    return content


def printJson(text_json):
    print(json.dumps(text_json, indent=4))


def writeJson(text_json):
    with codecs.open("content.json", "w", "utf-8-sig") as f:
        f.write(json.dumps(text_json, indent=4,
                sort_keys=True, ensure_ascii=False))
        f.close()


def getTemplatesToIgnore():
    templatesToIgnore = []
    url = baseUrl + "projects/" + str(projectId) + "/templates"
    querystring = {}
    response = requests.request(
        "GET", url, headers=headers, params=querystring)
    response_json = json.loads(response.text)
    for template in response_json["data"]:
        if("Guy Guide" in template["name"]):
            templatesToIgnore.append(template["id"])
    return templatesToIgnore


allItems = {}
allItems["data"] = []
itemsRaw = getItems()
folders = getFolders()
# printJson(itemsRaw["data"])
templatesToIgnore = getTemplatesToIgnore()
for item in itemsRaw["data"]:
    if(item["id"] in templatesToIgnore):
        continue
    # if("Guy Guide" in item["folder_name"]): continue
    for folder in folders["data"]:
        if(item["folder_uuid"] == folder["uuid"]):
            allItems["data"].append(getItemDetails(item["id"], folder["name"]))
            print(folder["uuid"])
allItems["data"].sort(key=lambda json: json['name'])
writeJson(allItems)
