const ResourceService = () => {
    const baseApiUrl = 'https://localhost:44357';

    const getResourceList = (success) => {
        $.getJSON(baseApiUrl + '/api/resources', success);
    };

    const getResourceById = (id, success) => {
        $.getJSON(baseApiUrl + '/api/resources/' + id, success);
    };

    const createResource = (model, success) => {
        $.post({
            url: baseApiUrl + '/api/resources',
            contentType: 'application/json',
            data: JSON.stringify(model),
            success
        });
    };

    const deleteResource = (id, success) => {
        $.ajax({
            url: baseApiUrl + '/api/resources/' + id,
            method: 'delete',
            success
        });
    };

    return {
        getResourceList,
        getResourceById,
        createResource,
        deleteResource
    };
};

const resourceService = ResourceService();