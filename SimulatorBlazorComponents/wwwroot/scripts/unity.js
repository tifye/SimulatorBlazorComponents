window.setupUnityInstance = async () => {
    var buildUrl = "Build";
    var config = {
        dataUrl: buildUrl + "/Build.data.unityweb",
        frameworkUrl: buildUrl + "/Build.framework.js.unityweb",
        codeUrl: buildUrl + "/Build.wasm.unityweb",
        streamingAssetsUrl: "StreamingAssets",
        companyName: "DefaultCompany",
        productName: "GSWebObserverUnity",
        productVersion: "0.1",
        matchWebGLToCanvasSize:false
    };

    try {
        const unityInstance = await createUnityInstance(document.querySelector("#unity-canvas"), config);
        window.unityInstance = unityInstance;

        // For Unity Bridge
        window.SendMessageToUnity = handleMessageToUnity;
    } catch (error) {
        alert(error);
        return false;
    }
    
    return true;
}

window.callOnInstance = async (functionName, params) => {
    //console.log(`Calling ${functionName} on unityInstance with params: ${params}`);
    unityInstance[functionName](...params);
}

window.quitUnityInstance = async () => {
    if (window.unityInstance === null) return;
    await window.unityInstance.Quit();
}

const handleMessageToUnity = (gameObject, unityMethodName, params) => {
    //console.log(gameObject, unityMethodName, params);
    if (params === null || params.length <= 0) {
        unityInstance.Module.SendMessage(gameObject, unityMethodName);
    }
    else {
        unityInstance.Module.SendMessage(gameObject, unityMethodName, ...params);
    }    
    return ""
}
