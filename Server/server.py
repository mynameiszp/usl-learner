from flask import Flask, request, jsonify
import psycopg2
from psycopg2.extras import RealDictCursor

app = Flask(__name__)

# Database connection
def get_db_connection():
    conn = psycopg2.connect(
        dbname="unitydb",
        user="admin",
        password="admin",
        host="localhost",
        port="5432"
    )
    return conn

# Function to execute query and return results
def query_db(query, params=None, fetch=False, fetchone=False):
    conn = get_db_connection()
    with conn.cursor(cursor_factory=RealDictCursor) as cur:
        cur.execute(query, params)
        if fetchone:
            result = cur.fetchone()
            conn.close()
            return result
        elif fetch:
            result = cur.fetchall()
            conn.close()
            return result
        conn.commit()
        conn.close()

# ---------------- GET ROUTES ---------------- #

@app.route('/players', methods=['GET'])
def get_players():
    return jsonify(query_db("SELECT * FROM players", fetch=True))

@app.route('/dictionaries', methods=['GET'])
def get_dictionaries():
    return jsonify(query_db("SELECT * FROM dictionaries", fetch=True))

@app.route('/words', methods=['GET'])
def get_words():
    return jsonify(query_db("SELECT * FROM words", fetch=True))

@app.route('/levels', methods=['GET'])
def get_levels():
    return jsonify(query_db("SELECT * FROM levels", fetch=True))

@app.route('/levelsWords', methods=['GET'])
def get_levels_words():
    return jsonify(query_db("SELECT * FROM levelsWords", fetch=True))

# Route for getting player by userId
@app.route('/players/<user_id>', methods=['GET'])
def get_player_by_user_id(user_id):
    player = query_db('SELECT * FROM players WHERE userId = %s', (user_id,), fetchone=True)
    if player is None:
        return jsonify({'error': 'Player not found'}), 404
    return jsonify(player)

# ---------------- POST ROUTES ---------------- #

@app.route('/players', methods=['POST'])
def add_player():
    data = request.json
    query_db("INSERT INTO players (userid, name, curlevel, score) VALUES (%s, %s, %s, %s)",
             (data['userid'], data['name'], data['curlevel'], data['score']))
    return jsonify({"status": "player added"}), 201

@app.route('/dictionaries', methods=['POST'])
def add_dictionary():
    data = request.json
    query_db("INSERT INTO dictionaries (name) VALUES (%s)", (data['name'],))
    return jsonify({"status": "dictionary added"}), 201

@app.route('/words', methods=['POST'])
def add_word():
    data = request.json
    query_db("INSERT INTO words (name, dictionaryid) VALUES (%s, %s)",
             (data['name'], data['dictionaryid']))
    return jsonify({"status": "word added"}), 201

@app.route('/levels', methods=['POST'])
def add_level():
    data = request.json
    query_db("INSERT INTO levels (level, points) VALUES (%s, %s)",
             (data['level'], data['points']))
    return jsonify({"status": "level added"}), 201

@app.route('/levelsWords', methods=['POST'])
def add_level_word():
    data = request.json
    query_db("INSERT INTO levelsWords (levelid, wordid) VALUES (%s, %s)",
             (data['levelid'], data['wordid']))
    return jsonify({"status": "level-word link added"}), 201

# ---------------- PUT ROUTES ---------------- #

@app.route('/players/<int:id>', methods=['PUT'])
def update_player(id):
    data = request.json
    query_db("UPDATE players SET curlevel = %s, score = %s WHERE id = %s",
             (data['curlevel'], data['score'], id))
    return jsonify({"status": "player updated"}), 200

@app.route('/players/<int:id>/name', methods=['PUT'])
def update_player_name(id):
    data = request.json
    query_db("UPDATE players SET name = %s WHERE id = %s", (data['name'], id))
    return jsonify({"status": "player name updated"}), 200

# ---------------- DELETE ROUTES ---------------- #
@app.route('/players/<int:id>', methods=['DELETE'])
def delete_player(id):
    query_db("DELETE FROM players WHERE id = %s", (id,))
    return jsonify({"status": "player deleted"}), 200

# ---------------- RUN SERVER ---------------- #

if __name__ == '__main__':
    app.run(host='0.0.0.0', port=5001)