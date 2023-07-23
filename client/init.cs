/**
* <author>Christophe Roblin</author>
* <url>https://github.com/LiF-x</url>
* <credits>https://github.com/LiF-x</credits>
* <description>Public repository to let everyone help on core functionality added to the LiFx serverautoloader</description>
* <license>GNU GENERAL PUBLIC LICENSE Version 3, 29 June 2007</license>
*/

exec("./jettison.cs");
exec("./sha256.cs");
if (!isObject(LiFx))
{
    new ScriptObject(LiFx)
    {
    };
}
if (!isObject(LiFxAutoload))
{
    new ScriptObject(LiFxAutoload)
    {
    };
}
$LiFx::debug = 1;
$LiFx::Version = "v3.0.0";
$LiFx::autojoin = "yolauncher/autojoin.cs";
$LiFx::kb = "https://kb.lifxmod.com/";
if (!IsDirectory("mods/"))
{
    createPath("mods/");
}
if ($LiFx::debug)
{
    $LiFx::Version = $LiFx::Version SPC $LiFx::debug;
}
$LiFx::hooks::onDatablockLoad = JettisonArray("onDatablockLoad");
$LiFx::hooks::onMaterialsLoad = JettisonArray("onMaterialsLoad");
$LiFx::hooks::onDatablockLoaded = JettisonArray("onDatablockLoaded");
$LiFx::hooks::onMaterialsLoaded = JettisonArray("onMaterialsLoaded");
$LiFx::hooks::onInitialized = JettisonArray("onInitialized");
$LiFx::hooks::onInitClientDone = JettisonArray("onInitClientDone");
$LiFx::hooks::beforeInitClientDone = JettisonArray("beforeInitClientDone");
$LiFx::hooks::mods = JettisonArray("mods");
$LiFx::zipFiles = JettisonArray("zipFiles");
singleton GuiControlProfile(LiFxGuiTextProfileCenter : GuiTextProfile)
{
    justify = "center";
    fontType = "Arial";
    fontSize = 24;
    FontColor = "0 0 0";
};
singleton GuiControlProfile(LiFxMainMenuWindowJoinButtonProfile : MainMenuWindowButtonsProfile)
{
    fontSize = 36;
    COLOR = "255 255 255";
};
singleton GuiControlProfile(LiFxGuiMainMenuTextProfileCenter : GuiTextProfile)
{
    justify = "center";
    fontType = "Arial";
    fontSize = 18;
    FontColor = "255 255 255";
};
package LiFx
{
    function LiFx::debugEcho(%message)
    {
        if ($LiFx::debug)
        {
            echo(%message);
        }
    }
    function LiFx::init()
    {
        echo("lifx init");
        if (isObject(LiFxShowVersionInfo))
        {
            LiFxShowVersionInfo.delete();
        }
        if ($cmYO)
        {
            $LiFxShowVersionInfoLoop = LiFx.schedule(500, "LiFxShowVersionInfo");
        }
        if (!IsDirectory("mods"))
        {
            createPath("mods");
        }
        if (!IsDirectory("mods/LiFx"))
        {
            createPath("mods/LiFx");
        }
        LiFx::registerCallback($LiFx::hooks::onInitialized, addKB, LiFx);
    }
    function LiFx::autojoin()
    {
        cancel($LiFxAutojoinLoop);
        $LiFxAutojoinLoop = LiFx.schedule(500, "autojoin");
        if (isObject(MainMenuMultiplayerWindow))
        {
            cancel($LiFxAutojoinLoop);
            LiFx::executeCallback($LiFx::hooks::onInitialized, %this, %obj);
            if (isFile($LiFx::autojoin))
            {
                exec($LiFx::autojoin);
            }
        }
    }
    function LiFx::addKB()
    {
        moveMapbind(keyboard, "f2", "LiFxOpenWeb");
        moveMapsave($bindingsConfig);
    }
    function LiFxOpenWeb(%val)
    {
        if (isObject($LiFx::wp) && $LiFx::wp.isActive())
        {
            return;
        }
        if (%val)
        {
            if (!($LiFx::kb $= ""))
            {
                LiFx::LiFxWeb($LiFx::kb, $LiFx::kb);
            }
        }
    }
    function LiFx::LiFxWeb(%url, %text)
    {
        $LiFx::wp = openWeb(%url);
        $LiFx::wptext = %text;
        $LiFx::wpcanMove = 1;
        $LiFx::wpcanSave = 1;
        $LiFx::wpPOSITIONx = PlayGuiextentx * 01;
        $LiFx::wpextentx = PlayGuiextentx * 06;
        $LiFx::wpPOSITIONy = PlayGuiextenty * 01;
        $LiFx::wpextenty = PlayGuiextenty * 08;
        $LiFx::wpcanGoBack = 1;
        $LiFx::wpcanGoForward = 1;
    }
    function LiFx::LiFxShowVersionInfo()
    {
        cancel($LiFxShowVersionInfoLoop);
        $LiFxShowVersionInfoLoop = LiFx.schedule(500, "LiFxShowVersionInfo");
        if (isObject(MainMenuMultiplayerWindow) && !isObject(LiFxGuiShowVersionInfo))
        {
            MainMenuMultiplayerWindow.add(LiFx::LiFxShowVersionInfoText());
        }
    }
    function LiFx::LiFxShowVersionInfoText()
    {
        %guiContent = new GuiControl(LiFxGuiShowVersionInfo)
        {
            POSITION = "05% 5%";
            extent = "384 128";
            minExtent = "8 2";
            horizSizing = "right";
            vertSizing = "bottom";
            profile = "LiFxGuiTextProfileCenter";
            visible = 1;
            active = 1;
            tooltipProfile = "GuiToolTipProfile";
            hovertime = 1000;
            isContainer = 1;
            canSave = 1;
            canSaveDynamicFields = 1;
            new GuiTextCtrl("")
            {
                text = "" @ $LiFx::Version;
                maxLength = 10;
                margin = "0 0 0 0";
                padding = "0 0 0 0";
                anchorTop = 1;
                anchorBottom = 0;
                anchorLeft = 1;
                anchorRight = 0;
                POSITION = "140 64";
                extent = "130 20";
                minExtent = "8 2";
                horizSizing = "right";
                vertSizing = "bottom";
                profile = "LiFxGuiTextProfileCenter";
                visible = 1;
                active = 1;
                tooltipProfile = "GuiToolTipProfile";
                hovertime = 1000;
                isContainer = 1;
                canSave = 1;
                canSaveDynamicFields = 0;
                COLOR = "0 0 0 0";
            };
        };
        return %guiContent;
    }
    function peerCmdLiFxCRC(%server, %file)
    {
        %hash = getFileCRC(%file);
        LiFx::debugEcho("File hash:" SPC %file SPC %hash);
        ServerConnectionsendCommand('LiFxCRC', %hash);
    }
    function peerCmdLiFxSHA256(%server, %file)
    {
        %hash = LiFxSHA256::hashMain(%file);
        LiFx::debugEcho("File hash:" SPC %file SPC %hash);
        ServerConnectionsendCommand('LiFxSHA256', %file, %hash);
    }
    function LiFx::registerCallback(%callbackArray, %function, %object)
    {
        echo(%callbackArray, %function, %object);
        if (%callbackArray.Length > 0)
        {
            foreach ( %f in %callbackArray)
            {
                if (%f == %function)
                {
                    return;
                }
            }
        }
        LiFx::debugEcho(%callbackArray.getName() SPC %function SPC %object);
        if (isObject(%object))
        {
            %callbackArray.push("string", %object @ "|" @ %function);
        }
        else
        {
            %callbackArray.push("string", %function);
        }
    }
    function LiFx::executeCallback(%array)
    {
        LiFx::executeCallback(%array, null, null, null, null, null);
    }
    function LiFx::executeCallback(%array, %ar1)
    {
        LiFx::executeCallback(%array, %ar1, null, null, null, null);
    }
    function LiFx::executeCallback(%array, %ar1, %ar2, %ar3, %ar4, %ar5)
    {
        if (%array.Length > 0)
        {
            %i = 0;
            while (%i < %array.Length)
            {
                %data = %array.value[%i];
                LiFx::debugEcho("Callback attempt on: " @ %data);
                if (isFunction(%data))
                {
                    call(%data, %ar1, %ar2, %ar3, %ar4, %ar5);
                }
                else
                {
                    if (strpos(%data, "|") > 0)
                    {
                        %func = nextToken(%data, "obj", "|");
                        if (isObject(%obj) && %obj.isMethod(%func))
                        {
                            LiFx::debugEcho(%obj.call(%func, %ar1, %ar2, %ar3, %ar4, %ar5));
                        }
                    }
                }
                %i = %i + 1;
            }
        }
    }
    function LiFx::registerExecCallback(%callbackArray, %object)
    {
        if (%callbackArray.Length > 0)
        {
            foreach ( %f in %callbackArray)
            {
                if (%f == %object)
                {
                    return;
                }
            }
        }
        if (isObject(%object))
        {
            %callbackArray.push("string", %object);
        }
    }
    function LiFx::getLeafFolder(%filePath)
    {
        %token = nextToken(nextToken(%filePath, "", "/"), "", "/");
        %length = strlen(%filePath);
        %leafFolder = getSubStr(%filePath, 0, %length - strlen(%token));
        return %leafFolder;
    }
    function LiFx::executeDatablockCallback()
    {
        if ($LiFx::hooks::onDatablockLoaded.Length > 0)
        {
            %i = 0;
            while (%i < $LiFx::hooks::onDatablockLoaded.Length)
            {
                %data = $LiFx::hooks::onDatablockLoaded.value[%i];
                if (isObject(%data))
                {
                    %leafFolder = LiFx::getLeafFolder(%data.call("path"));
                    LiFx::loadRecursivelyInFolder(%leafFolder, "datablockcs");
                }
                %i = %i + 1;
            }
        }
    }

    function LiFx::executeMaterialsCallback()
    {
        if ($LiFx::hooks::onMaterialsLoaded.Length > 0)
        {
            %i = 0;
            while (%i < $LiFx::hooks::onMaterialsLoaded.Length)
            {
                %data = $LiFx::hooks::onMaterialsLoaded.value[%i];
                if (isObject(%data))
                {
                    %leafFolder = LiFx::getLeafFolder(%data.call("path"));
                    LiFx::loadRecursivelyInFolder(%leafFolder, "materials.cs");
                }
                %i = %i + 1;
            }
        }
    }
};

package LiFx
{
    function LiFx::loadModsRecursivelyInFolder(%rootFolder)
    {
        LiFx::debugEcho(%rootFolder);
        LiFx::loadRecursivelyInFolder(%rootFolder, "cmod.cs");
        return 1;
    }
    function LiFx::loadZipModsRecursivelyInFolder(%rootFolder)
    {
        LiFx::loadZipRecursivelyInFolder(%rootFolder @ "mods/", "*zip");
        LiFx::loadZipRecursivelyInFolder(%rootFolder @ "modpack/", "*zip");
        if ($LiFx::zipFiles.Length > 0)
        {
            %i = 0;
            while (%i < $LiFx::zipFiles.Length)
            {
                %zip = $LiFx::zipFiles.value[%i];
                LiFx::debugEcho("Executing zip:" SPC %zip);
                LiFx::loadRecursivelyInFolder(%zip, "cmod.cs");
                %i = %i + 1;
            }
        }
        return 1;
    }
    function LiFx::loadRecursivelyInFolder(%rootFolder, %pattern)
    {
        LiFx::debugEcho(%rootFolder SPC %pattern);
        if (!((%rootFolder $= "")) && !((getSubStr(%rootFolder, strlen(%rootFolder) - 1) $= "/")))
        {
            %rootFolder = %rootFolder @ "/";
        }
        %findPattern = %rootFolder @ "*/" @ %pattern;
        %file = findFirstFileMultiExpr(%findPattern @ "dso", 1);
        while (!(%file $= ""))
        {
            %csFileName = getSubStr(%file, 0, strlen(%file) - 4);
            if (!isFile(%csFileName))
            {
                LiFx::debugEcho(%csFileName);
                exec(%csFileName);
            }
            %file = findNextFileMultiExpr(%findPattern @ "dso");
        }
        %file = findFirstFileMultiExpr(%findPattern, 1);
        while (!(%file $= ""))
        {
            LiFx::debugEcho(%file);
            exec(%file);
            %file = findNextFileMultiExpr(%findPattern);
        }
        return 1;
    }
    function LiFx::loadZipRecursivelyInFolder(%rootFolder, %pattern)
    {
        LiFx::debugEcho(%rootFolder SPC %pattern);
        if (!((%rootFolder $= "")) && !((getSubStr(%rootFolder, strlen(%rootFolder) - 1) $= "/")))
        {
            %rootFolder = %rootFolder @ "/";
        }
        %filePatterns = %rootFolder @ "*/" @ %pattern;
        %fullPath = findFirstFileMultiExpr(%filePatterns, 1);
        while (!(%fullPath $= ""))
        {
            %zipFolder = getSubStr(%fullPath, 0, strlen(%fullPath) - 4);
            if ($LiFx::zipFiles.Length > 0)
            {
                foreach ( %f in $LiFx::zipFiles)
                {
                    if (%f == %zipFolder)
                    {
                        return;
                    }
                }
            }
            LiFx::debugEcho("Adding to zip array" SPC %fullPath);
            LiFx::debugEcho(%zipFolder);
            $LiFx::zipFiles.push("string", %zipFolder);
            %fullPath = findNextFileMultiExpr(%filePatterns);
        }
        return 1;
    }
    function initClient()
    {
        if (((LiFx::loadModsRecursivelyInFolder("yolauncher/mods") && LiFx::loadZipModsRecursivelyInFolder("yolauncher/")) && LiFx::loadModsRecursivelyInFolder("yolauncher/modpack")) && LiFx::loadModsRecursivelyInFolder("mods"))
        {
            LiFx::debugEcho("\n Calling mods");
            LiFx::executeCallback($LiFx::hooks::mods);
        }
        echo("\n--------- Initializing " @ $appName @ ": Client Scripts ---------");
        if ($cmYO && isScriptFile("scripts/yo.cs"))
        {
            exec("scripts/yo.cs");
        }
        exec("scripts/cm_config.cs");
        CmConfiguration_init();
        if (isScriptFile("art/gui/customProfiles.cs"))
        {
            exec("art/gui/customProfiles.cs");
        }
        initBaseClient();
        loadMaterials();
        LiFx::executeMaterialsCallback();
        LiFx::executeCallback($LiFx::hooks::onMaterialsLoad, %this);
        exec("core/art/datablocks/datablockExec.cs");
        exec("art/datablocks/datablockExec.cs");
        LiFx::executeDatablockCallback();
        LiFx::executeCallback($LiFx::hooks::onDatablockLoad, %this);
        SubstanceManager_init();
        exec("scripts/cm_substances.cs");
        reloadGatheringInfo();
        if (!initSkillsManager())
        {
            error("Fatal: Can\'t load skills/abilities Terminating");
            messageBox("Error starting up", "Can\'t startup: failed to load skills/abilities", Ok, stop);
            quit();
            return;
        }
        if (!initTitlesManager())
        {
            error("Fatal: Can\'t load titles Terminating");
            messageBox("Error starting up", "Can\'t startup: failed to load titles", Ok, stop);
            quit();
            return;
        }
        initTooltipTemplateManager();
        exec("gui/scripts/tooltipManager.cs");
        fillTooltipTemplate();
        exec("art/gui/defaultGameProfiles.cs");
        exec("art/gui/PlayGui.gui");
        exec("gui/forms/guiSubtitlesTheoraCtrl.gui");
        exec("gui/forms/MainMenuMultiplayerWindow.gui");
        exec("gui/forms/CreateWorldWindow.gui");
        exec("gui/forms/JoinWorldWindow.gui");
        exec("art/gui/helpDialog.gui");
        exec("gui/forms/MessageBoxPasswordDlg.gui");
        exec("gui/forms/MessageBoxDirectConnect.gui");
        exec("art/gui/joinServerDlg.gui");
        exec("scripts/gui/playGui.cs");
        exec("art/gui/splitStackItem.gui");
        exec("art/gui/prospectingRadiusDialog.gui");
        exec("scripts/gui/InventoryGui.cs");
        exec("scripts/gui/prospectingRadiusDialog.cs");
        exec("scripts/gui/cmCraftworkBrewingTankOptionsDlg.gui");
        exec("scripts/gui/cmAcceptDialog.gui");
        exec("gui/scripts/gui.cs");
        exec("/client.cs");
        exec("/game.cs");
        exec("/missionDownload.cs");
        exec("/serverConnection.cs");
        exec("/cServer.cs");
        exec("/shaders.cs");
        exec("scripts/client/defaultbindCommands.cs");
        exec("scripts/client/horsebindCommands.cs");
        if (isScriptFile("scripts/client/developersbind.cs"))
        {
            exec("scripts/client/developersbind.cs");
        }
        if (isFile($bindingsConfig))
        {
            exec($bindingsConfig);
        }
        else
        {
            exec("scripts/client/defaultbind.cs");
        }
        exec("scripts/client/defaultbindupdate.cs");
        setNetPort(0, 0);
        setDefaultFov($pref::Player::defaultFov);
        setFov($pref::player::CurrentFOV);
        setZoomSpeed($pref::Player::zoomSpeed);
        StreamGroup::addSortingGroup(1, $pref::TS::renderingDistanceSmall * 2, 1);
        StreamGroup::addSortingGroup(2, $pref::TS::renderingDistanceBig * 2, 15);
        StreamGroup::addSortingGroup(3, $pref::TS::renderingDistanceHuge * 2, 2);
        exec("/cm_lightProto.cs");
        exec("art/forest/treeDatablocks.cs");
        exec("scripts/web/GuiBrowserWindow.gui");
        loadTutorial();
        initManagers();
        initAdminLandsAbilities();
        exec("art/sound/music/music.cs");
        removeFolderWithAllFiles("art/Textures/Heraldry/Cache");
        LiFx::executeCallback($LiFx::hooks::beforeInitClientDone, %this);
        schedule(256, Canvas, dumpSysInfo, "initClient");
        onInitClientDone();
        LiFx::executeCallback($LiFx::hooks::onInitClientDone, %this);
        LiFx::autojoin();
    }
    function initManagers()
    {
        initSoundsManager();
        initLoadGuildsClient();
        createLandsManager();
        initLoadClaimRules();
        initSkinManager();
        loadAttackAnimationsXml();
        initXsollaManager();
    }
    function peerCmdDisableChat(%server, %value)
    {
      %disableChat = %value;
    }
    function cmJoinDefaultChats()
    {
        if(!%disableChat) {
          cmChatJoin("@");
          cmChatJoin("");
        }
    }
    function showWelcomeMessage()
    {
        if (($pref::hideWelcomeMessage != 1) && $isNewbieWorld)
        {
            cmShowMessage(697);
            $pref::hideWelcomeMessage = 1;
        }
    }
};

activatePackage(LiFx);
LiFx::init();

